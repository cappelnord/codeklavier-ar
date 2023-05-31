using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class FerniBranchIntensity : IntensityBehaviour
{
    private MeshRenderer meshRenderer;

    public Color Grey;
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
        if(!Gen.IntensityHasChanged && !firstUpdate) return;
        
        float ci = Gen.ColorIntensity;
        float i = Gen.Intensity * 0.4f;
        float im = 1.0f + i;

        Color c = Color.Lerp(Green, Grey, ci);

        meshRenderer.sharedMaterial.SetColor("_Color", new Color(c.r * im, c.g * im, c.b * im));

        firstUpdate = false;
        
    }
}
}
