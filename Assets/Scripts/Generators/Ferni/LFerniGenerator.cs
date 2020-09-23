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
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float displace = 0.0f;

        Dictionary<long, FerniNode> positions = new Dictionary<long, FerniNode>();

        int generation = 0;
        float jointScale = 0.05f;
        float branchRadius = 0.02f;
        float deltaDisplace = 1.25f;

        foreach (List<ProcessUnit> data in lsys.Units)
        {

            float pos = data.Count / -2.0f + 0.5f;
            foreach (ProcessUnit unit in data)
            {
                char symbol = unit.Content;
                pos += 1.0f;

                positions[unit.Id] = new FerniNode(new Vector3(pos / 20.0f, displace / 3.0f, 0.0f), generation, jointScale, branchRadius);
            }

            displace += deltaDisplace;

            generation++;
            jointScale *= 0.8f;
            branchRadius *= 0.8f;
            deltaDisplace *= 0.75f;

        }


        foreach (List<ProcessUnit> data in lsys.Units)
        {
            foreach (ProcessUnit unit in data)
            {
                
                if (positions.ContainsKey(unit.Id) && unit.Content != '0' && unit.Children.Count > 0)
                {
                    GameObject obj = Object.Instantiate(JointPrefab, transform);
                    FerniNode node = positions[unit.Id];
                    obj.transform.localPosition = node.Position;
                    obj.transform.localScale = new Vector3(node.JointScale, node.JointScale, node.JointScale);
                }
                

                foreach (ProcessUnit child in unit.Children)
                {
                    if (unit.Content != '0' && child.Content != '0')
                    {
                        if (positions.ContainsKey(unit.Id) && positions.ContainsKey(child.Id))
                        {

                            FerniNode from = positions[unit.Id];
                            FerniNode to  = positions[child.Id];

                            Vector3 between = to.Position - from.Position;
                            float distance = between.magnitude;

                            GameObject obj = Spawn(transform, distance, from.BranchRadius); ;

                            obj.transform.localPosition = from.Position + (between * 0.5f);
                            obj.transform.localRotation = Quaternion.LookRotation(between) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
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
        float radiusUp = radius * 0.25f;
        float radiusDown = radius * 0.5f;

        float lengthDown = length / 2.0f;
        float lengthUp = length / 2.0f;

        float squish = 0.5f;
        float squishUp = 1.0f;
        float squishDown = 1.0f;

        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, BranchMaterial);

    }
}
