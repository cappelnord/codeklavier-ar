using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Could be struct for less GC
public class FerniNode
{
    public Vector3 Position;
    public int Generation;
    public float JointScale;
    public float BranchRadius;

    public FerniNode(Vector3 position, int generation, float jointScale, float branchRadius)
    {
        Position = position;
        Generation = generation;
        JointScale = jointScale;
        BranchRadius = branchRadius;
    }
}

public class LFerniGenerator : LGenerator
{

    public Material BranchMaterial;
    public GameObject JointPrefab;

    private WedgeMeshGen wedgeMeshGen;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        wedgeMeshGen = WedgeMeshGen.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        float displace = 0.0f;

        Dictionary<long, FerniNode> nodes = new Dictionary<long, FerniNode>();

        int generation = 0;
        float jointScale = 0.1f;
        float branchRadius = 0.04f;
        float deltaDisplace = 1.25f;

        foreach (List<ProcessUnit> data in lsys.Units)
        {

            float pos = data.Count / -2.0f + 0.5f;
            foreach (ProcessUnit unit in data)
            {
                char symbol = unit.Content;
                pos += 1.0f;

                nodes[unit.Id] = new FerniNode(new Vector3(pos / 20.0f, displace / 2.0f, 0.0f), generation, jointScale, branchRadius);
            }

            displace += deltaDisplace;

            generation++;
            jointScale *= 0.7f;
            branchRadius *= 0.7f;
            deltaDisplace *= 0.75f;

        }

        foreach (FerniNode node in nodes.Values)
        {
            Vector3 position = node.Position;
            node.Position = new Vector3(position.x + Mathf.Sin((position.x) * 8.2f) * 0.2f, position.y + Mathf.Sin(position.x  * 2.3f) * 0.6f, Mathf.Cos((position.x) * 7.5f) * 0.2f);
        }


        foreach (List<ProcessUnit> data in lsys.Units)
        {
            foreach (ProcessUnit unit in data)
            {
                
                if (nodes.ContainsKey(unit.Id) && unit.Content != '0' && unit.Children.Count > 0)
                {
                    
                    GameObject obj = Object.Instantiate(JointPrefab, transform);
                    FerniNode node = nodes[unit.Id];
                    obj.transform.localPosition = node.Position;
                    LifeBehaviour lb = obj.AddComponent<LifeBehaviour>() as LifeBehaviour;
                    lb.TargetScale = new Vector3(node.JointScale, node.JointScale, node.JointScale);
                    lb.GrowStartTime = Time.time + (node.Generation * 0.5f);
                    lb.GrowTime = 0.5f;

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

                            GameObject obj = Spawn(transform, distance, from.BranchRadius); ;

                            obj.transform.localPosition = from.Position;
                            obj.transform.localRotation = Quaternion.LookRotation(between) * Quaternion.Euler(90.0f, 0.0f, 0.0f);

                            LifeBehaviour lb = obj.AddComponent<LinearLifeBehaviour>() as LifeBehaviour;
                            lb.TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
                            lb.GrowStartTime = Time.time + (from.Generation * 0.5f);
                            lb.GrowTime = 0.5f;

                        }
                    }
                }
            }
        }
    }

    public GameObject Spawn(Transform parent, float length, float radius)
    {
        const int sides = 16;

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
