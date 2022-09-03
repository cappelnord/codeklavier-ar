using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrunastriBranchIntensity : IntensityBehaviour
{
    private MeshRenderer meshRenderer;

    public Color Grey;
    public Color Green;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float ci = Gen.ColorIntensity;
        float i = Gen.Intensity * 0.4f;

        Color c = Color.Lerp(Green, Grey, ci);
        float im = 1.0f + i;

        meshRenderer.material.SetColor("_Color", new Color(c.r * im, c.g * im, c.b * im));
        */
    }
}
