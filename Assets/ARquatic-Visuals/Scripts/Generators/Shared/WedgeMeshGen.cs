﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class WedgeMeshGen : MeshGen
{

    private static WedgeMeshGen instance;
    public static WedgeMeshGen Instance()
    {
        if (instance == null)
        {
            instance = GameObject.Find("MeshGen").GetComponent<WedgeMeshGen>();
        }

        return instance;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

    }

    public GameObject GetWedgeObject(int sides, float radiusCenter, float radiusUp, float lengthUp, float radiusDown, float lengthDown, float squish, float squishUp, float squishDown, Transform transform, Material material)
    {
        Mesh mesh = GetMesh(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown);
        return GetObject(mesh, transform, material);
    }

    public Mesh GetMesh(int sides, float radiusCenter, float radiusUp, float lengthUp, float radiusDown, float lengthDown, float squish, float squishUp, float squishDown)
    {
        
        float thresh = 0.00001f;
        string key = $"{sides}-{Mathf.RoundToInt(radiusCenter * 1000.0f)}-{Mathf.RoundToInt(radiusDown * 1000.0f)}-{Mathf.RoundToInt(lengthDown * 1000)}-{Mathf.RoundToInt(radiusUp * 1000.0f)}-{Mathf.RoundToInt(lengthUp * 1000)}-{Mathf.RoundToInt(squish * 1000)}-{Mathf.RoundToInt(squishUp * 1000)}-{Mathf.RoundToInt(squishDown * 1000)}";

        Mesh cachedMesh = GetCachedMesh(key);
        if (cachedMesh != null) return cachedMesh;

        int sidespp = sides + 1;

        // Vertices
        // Inner Ring + Endpoints
        int numVertices = sidespp + 2;
        // Sides
        int numTriangles = 2 * sidespp;

        bool upHasSurface = radiusUp >= thresh;
        bool downHasSurface = radiusDown >= thresh;

        // Down/Up Ring (if necessary)
        if (upHasSurface)
        {
            numVertices += sidespp;
            numTriangles += 2 * sidespp;
        }
        if(downHasSurface)
        {
            numVertices += sidespp;
            numTriangles += 2 * sidespp;
        }

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles * 3];

        // up, down
        vertices[0] = new Vector3(0.0f, lengthUp, 0.0f);
        vertices[1] = new Vector3(0.0f, -lengthDown, 0.0f);

        uvs[0] = new Vector2(0.5f, 1f);
        uvs[1] = new Vector2(0.5f, 0f);


        float deltaAngle = Mathf.PI * 2.0f / sides;
        float rad = 0.0f;

        int ringIndex = 2;
        int upRingIndex = ringIndex + sidespp;
        int downRingIndex = ringIndex + sidespp;
        if(upHasSurface)
        {
            downRingIndex = downRingIndex + sidespp;
        }


        // circles
        for(int i = 0; i <= sides; i++)
        {
            float ms = Mathf.Sin(rad);
            float mc = Mathf.Cos(rad);
            vertices[ringIndex + i] = new Vector3(ms * radiusCenter, 0.0f, mc * squish * radiusCenter);
            uvs[ringIndex + i] = new Vector2((float)i / sides, 0.5f);

            if (upHasSurface)
            {
                vertices[upRingIndex + i] = new Vector3(ms * radiusUp, lengthUp, mc * radiusUp * squishUp);
                uvs[upRingIndex + i] = new Vector2((float)i / sides, 1f);

            }
            if (downHasSurface)
            {
                vertices[downRingIndex + i] = new Vector3(ms * radiusDown, -lengthDown, mc * radiusDown * squishDown);
                uvs[downRingIndex + i] = new Vector2((float)i / sides, 0f);

            }

            rad += deltaAngle;
        }


        int tIndex = 0;

        // down
        if(!upHasSurface)
        {
            for(int i = 0; i < sidespp; i++)
            {
                triangles[tIndex] = ringIndex + i;
                triangles[tIndex + 1] = ringIndex + ((i + 1) % sidespp);
                triangles[tIndex + 2] = 0;
                tIndex += 3;
            }
        } else
        {
            for(int i = 0; i < sidespp; i++)
            {
                int ni = (i + 1) % sidespp;

                triangles[tIndex] = ringIndex + i;
                triangles[tIndex + 1] = ringIndex + ni;
                triangles[tIndex + 2] = upRingIndex + i;

                triangles[tIndex + 3] = ringIndex + ni;
                triangles[tIndex + 4] = upRingIndex + ni;
                triangles[tIndex + 5] = upRingIndex + i;

                triangles[tIndex + 6] = upRingIndex + i;
                triangles[tIndex + 7] = upRingIndex + ni;
                triangles[tIndex + 8] = 0;

                tIndex += 9;
            }
        }

        if(!downHasSurface)
        {
            for (int i = 0; i < sidespp; i++)
            {
                triangles[tIndex + 2] = ringIndex + i;
                triangles[tIndex + 1] = ringIndex + ((i + 1) % sidespp);
                triangles[tIndex] = 1;

                tIndex += 3;
            }
        } else
        {
            for (int i = 0; i < sidespp; i++)
            {
                int ni = (i + 1) % sidespp;

                triangles[tIndex + 2] = ringIndex + i;
                triangles[tIndex + 1] = ringIndex + ni;
                triangles[tIndex + 0] = downRingIndex + i;

                triangles[tIndex + 5] = ringIndex + ni;
                triangles[tIndex + 4] = downRingIndex + ni;
                triangles[tIndex + 3] = downRingIndex + i;

                triangles[tIndex + 8] = downRingIndex + i;
                triangles[tIndex + 7] = downRingIndex + ni;
                triangles[tIndex + 6] = 1;

                tIndex += 9;
            }
        }


        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        // mesh.RecalculateTangents();

        return CacheMesh(key, mesh);
        
    }

    // free cache
    public void OnDestroy() {
        instance = null;
    }
}
}