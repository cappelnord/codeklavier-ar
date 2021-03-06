using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrunastriFruitIntensity : IntensityBehaviour
{
    private Vector3 scale;
    private MeshRenderer meshRenderer;

    public Color Green;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float ci = Gen.ColorIntensity;
        float i = Gen.Intensity * 0.4f;
        float im = 1.0f + i;

        Color c = Color.Lerp(Green, KeyColor, ci);

        meshRenderer.material.SetColor("_Color", new Color(c.r * im, c.g * im, c.b * im));

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1.0f + (4.0f * i), 1.0f + (1.5f * i), 1.0f + (4.0f * i)));
    }
}
