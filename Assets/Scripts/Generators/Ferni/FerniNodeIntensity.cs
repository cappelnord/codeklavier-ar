using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerniNodeIntensity : IntensityBehaviour
{
    private Vector3 scale;
    private MeshRenderer meshRenderer;

    private Color green;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        green = new Color((80f / 255f) * 0.8f, (198f / 255f) * 0.8f, (29f / 255f) * 0.8f);

    }

    // Update is called once per frame
    void Update()
    {
        float ci = Gen.ColorIntensity;
        float i = Gen.Intensity * 0.4f;
        float im = 1.0f + i;

        Color c = Color.Lerp(green, KeyColor, ci);

        meshRenderer.material.SetColor("_Color", new Color(c.r * im, c.g * im, c.b * im));

        transform.localScale = transform.localScale * (1.0f + (5.0f * i));
    }
}
