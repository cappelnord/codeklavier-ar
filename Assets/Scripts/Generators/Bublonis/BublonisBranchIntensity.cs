using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BublonisBranchIntensity : IntensityBehaviour
{
    private MeshRenderer meshRenderer;

    private Color grey;
    private Color green;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        grey = new Color(0.8f, 0.8f, 0.8f);
        green = new Color((30f / 255f) * 0.8f, (120f / 255f) * 0.8f, (10f / 255f) * 0.8f);

    }

    // Update is called once per frame
    void Update()
    {
        float ci = Gen.ColorIntensity;
        float i = Gen.Intensity * 0.4f;
        float im = 1.0f + i;

        Color c = Color.Lerp(green, grey, ci);

        meshRenderer.material.SetColor("_Color", new Color(c.r * im, c.g * im, c.b * im));
    }
}
