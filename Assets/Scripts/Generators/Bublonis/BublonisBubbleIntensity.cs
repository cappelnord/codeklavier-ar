using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BublonisBubbleIntensity : IntensityBehaviour
{
    private Vector3 scale;
    new private MeshRenderer meshRenderer;

    private Color green;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        green = new Color(60f / 255f, 168f / 255f, 19f / 255f);

    }

    // Update is called once per frame
    void Update()
    {
        float ci = Gen.ColorIntensity;
        float i = Gen.Intensity * 0.2f;

        Color c = Color.Lerp(green, KeyColor, ci);

        meshRenderer.material.SetColor("_Color", new Color(c.r + i, c.g + i, c.b + i));

        transform.localScale = transform.localScale * (1.0f + (2.0f * i));
    }
}
