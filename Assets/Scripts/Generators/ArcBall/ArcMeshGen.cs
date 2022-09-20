using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.Visuals;

public class ArcMeshGen : MeshGen
{

    public int Resolution = 512;

    private static ArcMeshGen instance;
    public static ArcMeshGen Instance()
    {
        if(instance == null)
        {
            instance = GameObject.Find("MeshGen").GetComponent<ArcMeshGen>();
        }

        return instance;
    }


    public override void Start()
    {
        base.Start();
    }



    public GameObject GetArcObject(float angle, float filledToCenter, float width, Transform parent, Material material)
    {
        Mesh mesh = GetMesh(angle, filledToCenter, width);
        return GetObject(mesh, parent, material);
    }

    public Mesh GetMesh(float angle, float filledToCenter, float width)
    {
    	string key  = $"{Mathf.RoundToInt(angle*1000.0f)}-{Mathf.RoundToInt(width*1000)}-{Mathf.RoundToInt(filledToCenter * 100)}";

        Mesh cachedMesh = GetCachedMesh(key);
        if (cachedMesh != null) return cachedMesh;

        bool isFullCircle = angle >= ((2.0f * Mathf.PI) - 0.01f);

        int numSteps = (int) (Resolution * angle / (2.0 * Mathf.PI));
        int numVertices = numSteps * 4;
        int numTriangles = (2 * numVertices);


        // falls nicht geschlossen:

        // triangles: 2 on each endpiece, 2 for every 4 points, 4 sides
        //                     4                       2 * points

        // falls geschlossen:
        // + 8 triangles auf jeder Seite

        if(isFullCircle)
        {
            numTriangles += 8;
        } else
        {
            numTriangles += 4;
        }

        int numTriangleIndices = numTriangles * 3;

        // let's do the vertices: front-down, front-up, back-up, back-down

        Vector3[] vertices = new Vector3[numVertices];

        for(int i = 0; i < numSteps; i++)
        {
            int numStepsCalc = numSteps;
            if(!isFullCircle)
            {
                numStepsCalc -= 1;
            }
            float rad = (i / (float) numStepsCalc ) * angle;
            float mx = Mathf.Sin(rad);
            float my = Mathf.Cos(rad);
            float centerMul = (1.0f - filledToCenter);

            vertices[i * 4 + 0] = new Vector3(mx * centerMul, my * centerMul, width * 0.5f);
            vertices[i * 4 + 1] = new Vector3(mx, my, width * 0.5f);
            vertices[i * 4 + 2] = new Vector3(mx, my, width * -0.5f);
            vertices[i * 4 + 3] = new Vector3(mx * centerMul, my * centerMul, width * -0.5f);
        }

        int[] triangles = new int[numTriangleIndices];

        int tIndex = 0;

        // front
        if (!isFullCircle)
        {
            triangles[0] = 3;
            triangles[1] = 0;
            triangles[2] = 1;
            triangles[3] = 1;
            triangles[4] = 2;
            triangles[5] = 3;

            tIndex = 6;
        }

        for(int i = 0; i < (numSteps-1); i++)
        {
            int ni = i + 1;

            // front
            triangles[tIndex + 2] = (i * 4) + 0;
            triangles[tIndex + 1] = (i * 4) + 1;
            triangles[tIndex + 0] = (ni * 4) + 0;

            triangles[tIndex + 3] = (ni * 4) + 0;
            triangles[tIndex + 4] = (ni * 4) + 1;
            triangles[tIndex + 5] = (i * 4) + 1;
            tIndex += 6;

            // top
            triangles[tIndex + 2] = (i * 4) + 0 + 1; 
            triangles[tIndex + 1] = (i * 4) + 1 + 1;
            triangles[tIndex + 0] = (ni * 4) + 0 + 1;

            triangles[tIndex + 3] = (ni * 4) + 0 + 1;
            triangles[tIndex + 4] = (ni * 4) + 1 + 1;
            triangles[tIndex + 5] = (i * 4) + 1 + 1;
            tIndex += 6;

            // back
            triangles[tIndex + 2] = (i * 4) + 0 + 2;
            triangles[tIndex + 1] = (i * 4) + 1 + 2;
            triangles[tIndex + 0] = (ni * 4) + 0 + 2;

            triangles[tIndex + 3] = (ni * 4) + 0 + 2;
            triangles[tIndex + 4] = (ni * 4) + 1 + 2;
            triangles[tIndex + 5] = (i * 4) + 1 + 2;
            tIndex += 6;

            // down
            triangles[tIndex + 2] = (i * 4) + 0 + 3;
            triangles[tIndex + 1] = (i * 4) + 0;
            triangles[tIndex + 0] = (ni * 4) + 0 + 3;

            triangles[tIndex + 3] = (ni * 4) + 0 + 3;
            triangles[tIndex + 4] = (ni * 4) + 0;
            triangles[tIndex + 5] = (i * 4) + 0; 

            tIndex += 6;
        }

        if (!isFullCircle)
        {
            // back triangles
            triangles[tIndex + 5] = numVertices - 1;
            triangles[tIndex + 4] = numVertices - 4;
            triangles[tIndex + 3] = numVertices - 3;
            triangles[tIndex + 2] = numVertices - 3;
            triangles[tIndex + 1] = numVertices - 2;
            triangles[tIndex + 0] = numVertices - 1;
        } else
        {
            int i = numSteps-1;
            int ni = 0;

            // front
            triangles[tIndex + 2] = (i * 4) + 0;
            triangles[tIndex + 1] = (i * 4) + 1;
            triangles[tIndex + 0] = (ni * 4) + 0;

            triangles[tIndex + 3] = (ni * 4) + 0;
            triangles[tIndex + 4] = (ni * 4) + 1;
            triangles[tIndex + 5] = (i * 4) + 1;
            tIndex += 6;

            // top
            triangles[tIndex + 2] = (i * 4) + 0 + 1;
            triangles[tIndex + 1] = (i * 4) + 1 + 1;
            triangles[tIndex + 0] = (ni * 4) + 0 + 1;

            triangles[tIndex + 3] = (ni * 4) + 0 + 1;
            triangles[tIndex + 4] = (ni * 4) + 1 + 1;
            triangles[tIndex + 5] = (i * 4) + 1 + 1;
            tIndex += 6;

            // back
            triangles[tIndex + 2] = (i * 4) + 0 + 2;
            triangles[tIndex + 1] = (i * 4) + 1 + 2;
            triangles[tIndex + 0] = (ni * 4) + 0 + 2;

            triangles[tIndex + 3] = (ni * 4) + 0 + 2;
            triangles[tIndex + 4] = (ni * 4) + 1 + 2;
            triangles[tIndex + 5] = (i * 4) + 1 + 2;
            tIndex += 6;

            // down
            triangles[tIndex + 2] = (i * 4) + 0 + 3;
            triangles[tIndex + 1] = (i * 4) + 0;
            triangles[tIndex + 0] = (ni * 4) + 0 + 3;

            triangles[tIndex + 3] = (ni * 4) + 0 + 3;
            triangles[tIndex + 4] = (ni * 4) + 0;
            triangles[tIndex + 5] = (i * 4) + 0;

            tIndex += 6;
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return CacheMesh(key, mesh);
    }
}
