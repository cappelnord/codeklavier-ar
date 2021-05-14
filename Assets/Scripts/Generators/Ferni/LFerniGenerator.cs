using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Could be struct for less GC
public class FerniNode
{
    public Vector3 BasePosition;
    public Vector3 Position;
    public int Generation;
    public float JointScale;
    public float BranchRadius;
    public Transform Transform;

    public FerniNode(Vector3 position, int generation, float jointScale, float branchRadius)
    {
        BasePosition = position;
        Position = position;
        Generation = generation;
        JointScale = jointScale;
        BranchRadius = branchRadius;
    }
}

public class FerniBranch
{
    public FerniNode From;
    public FerniNode To;
    public Transform Transform;
    public float BaseDistance;
    public LinearLifeBehaviour LifeBehaviour;

    public FerniBranch(Transform transform, FerniNode from, FerniNode to, float baseDistance, LinearLifeBehaviour llb)
    {
        LifeBehaviour = llb;
        BaseDistance = baseDistance;
        Transform = transform;
        From = from;
        To = to;
    }

}

public class LFerniGenerator : LGenerator
{

    public Material BranchMaterial;
    public GameObject JointPrefab;

    private WedgeMeshGen wedgeMeshGen;

    private Dictionary<long, FerniNode> nodes;
    private List<FerniBranch> branches;

    private Color branchGreen;
    private Color nodeGreen;
    private Color branchGrey;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if(!Variety)
        {
            nodeGreen = new Color((80f / 255f) * 0.8f, (198f / 255f) * 0.8f, (29f / 255f) * 0.8f);
            branchGrey = new Color(0.0f, 0.0f, 0.0f);
            branchGreen = new Color((60f / 255f) * 0.8f, (220f / 255f) * 0.8f, (0f / 255f) * 0.8f);
        } else
        {
            nodeGreen = new Color((80f / 255f) * 0.8f, (158f / 255f) * 0.8f, (29f / 255f) * 0.6f);
            branchGrey = new Color(1.0f, 1.0f, 1.0f);
            branchGreen = new Color((60f / 255f) * 0.6f, (140f / 255f) * 0.6f, (0f / 255f) * 0.6f);
        }

        wedgeMeshGen = WedgeMeshGen.Instance();
    }

    protected override void Update()
    {
        base.Update();

        if(nodes != null)
        {
            foreach (FerniNode node in nodes.Values)
            {
                node.Position = node.BasePosition + ARquaticEnvironment.Instance.Current(node.BasePosition + Position) * (0.25f + (SpeedMultiplier * 0.25f)) * 0.5f;
                if (node.Transform != null) { 
                    node.Transform.localPosition = node.Position;
                }
            }

            foreach (FerniBranch branch in branches)
            {
                FerniNode from = branch.From;
                FerniNode to = branch.To;

                Vector3 between = to.Position - from.Position;
                float distance = between.magnitude;

                branch.Transform.localPosition = from.Position;
                branch.Transform.localRotation = Quaternion.LookRotation(between) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
                branch.Transform.localScale = branch.LifeBehaviour.OutputScale * (distance / branch.BaseDistance);
            }
        }
    }

    public override void Die(bool transformDeath = false)
    {
        nodes = null;
        branches = null;
        base.Die();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        nodes = new Dictionary<long, FerniNode>();
        branches = new List<FerniBranch>();

        if (IsEmpty()) return;


        float displace = 0.0f;

        int generation = 0;
        float jointScale = 0.0666f;
        float branchRadius = 0.02f;
        float deltaDisplace = 1.25f;

        if(Variety)
        {
            deltaDisplace = Random.Range(0.5f, 1.5f);
            jointScale = 0.05f;
            branchRadius *= 0.5f;
        }

        foreach (List<ProcessUnit> data in lsys.Units)
        {

            float pos = data.Count / -2.0f + 0.5f;
            foreach (ProcessUnit unit in data)
            {
                char symbol = unit.Content;
                pos += 1.0f;

                float posDiv = 20f;
                if(Variety)
                {
                    posDiv = 10f;
                }

                nodes[unit.Id] = new FerniNode(new Vector3(pos / posDiv, displace / 2f, 0.0f), generation, jointScale, branchRadius);
            }

            displace += deltaDisplace;

            generation++;
            jointScale *= 0.7f;
            branchRadius *= 0.7f;
            deltaDisplace *= 0.75f;

        }

        foreach (FerniNode node in nodes.Values)
        {
            Vector3 position = node.BasePosition;
            // base form
            if(!Variety)
            {
                node.BasePosition = new Vector3(position.x + Mathf.Sin((position.x) * 8.2f) * 0.2f, position.y + Mathf.Sin(position.x * 2.3f) * 0.6f, Mathf.Cos((position.x) * 7.5f) * 0.2f);
            } else
            {
                node.BasePosition = new Vector3(position.x + Mathf.Sin((position.x) * 9.2f) * 0.1f, position.y + Mathf.Sin(position.x * 2.3f) * 0.2f, Mathf.Cos((position.x) * 7.5f) * 0.1f);
            }
            node.Position = node.BasePosition;
        }


        foreach (List<ProcessUnit> data in lsys.Units)
        {
            foreach (ProcessUnit unit in data)
            {
                if (nodes.ContainsKey(unit.Id) && unit.Content != '0' && unit.Children.Count > 0)
                {
                    FerniNode node = nodes[unit.Id];
                    if (node.JointScale > 0.02f)
                    {
                        GameObject obj = Object.Instantiate(JointPrefab, transform);
                        node.Transform = obj.transform;
                        LifeBehaviour lb = obj.AddComponent<LifeBehaviour>() as LifeBehaviour;
                        lb.TargetScale = new Vector3(node.JointScale, node.JointScale, node.JointScale);
                        lb.GrowStartTime = Time.time + (node.Generation* 0.5f);
                        lb.GrowTime = 0.1f;

                        FerniNodeIntensity ib = obj.AddComponent<FerniNodeIntensity>() as FerniNodeIntensity;
                        ib.Gen = this;
                        ib.KeyColor = materialLookup.GetColor(unit.Content);
                        ib.Green = nodeGreen;
                    }
                }

                foreach (ProcessUnit child in unit.Children)
                {
                    if (unit.Content != '0' && child.Content != '0')
                    {
                        if (nodes.ContainsKey(unit.Id) && nodes.ContainsKey(child.Id))
                        {

                            FerniNode from = nodes[unit.Id];
                            FerniNode to  = nodes[child.Id];

                            Vector3 between = to.Position - from.Position;
                            float distance = between.magnitude;

                            GameObject obj = Spawn(transform, distance, from.BranchRadius);


                            LinearLifeBehaviour lb = obj.AddComponent<LinearLifeBehaviour>() as LinearLifeBehaviour;
                            lb.TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
                            lb.GrowStartTime = Time.time + (from.Generation * 0.5f);
                            lb.GrowTime = 0.5f;

                            FerniBranchIntensity ib = obj.AddComponent<FerniBranchIntensity>() as FerniBranchIntensity;
                            ib.Gen = this;
                            ib.Grey = branchGrey;
                            ib.Green = branchGreen;


                            branches.Add(new FerniBranch(obj.transform, from, to, distance, lb));
                        }
                    }
                }
            }
        }
    }

    public GameObject Spawn(Transform parent, float length, float radius)
    {
        const int sides = 12;

        float radiusCenter = radius;
        float radiusUp = radius * 0.7f;
        float radiusDown = radius * 0.5f;

        float lengthDown = length / 64.0f;
        float lengthUp = length;

        float squish = 0.5f;
        float squishUp = 1.0f;
        float squishDown = 1.0f;

        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, BranchMaterial);

    }
}
