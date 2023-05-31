using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class BublonisBubbleIntensity : IntensityBehaviour
{
    private Vector3 scale;
    private MeshRenderer meshRenderer;

    public Color Green;

    private bool firstUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        float i = Gen.Intensity * 0.4f;
        transform.localScale = transform.localScale * (1.0f + (2.0f * i));

        if(Gen.IntensityHasChanged || firstUpdate) {
            float ci = Gen.ColorIntensity;
            float im = 1.0f + i;

            Color c = Color.Lerp(Green, KeyColor, ci);

            meshRenderer.sharedMaterial.SetColor("_Color", new Color(c.r * im, c.g * im, c.b * im));

            firstUpdate = false;
        }
    }
}
}
