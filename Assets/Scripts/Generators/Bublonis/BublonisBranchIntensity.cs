using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BublonisBranchIntensity : IntensityBehaviour
{
    new private MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float ci = Gen.ColorIntensity;
        renderer.material.SetColor("_Color", new Color(80f / 255f + ci, 198f / 255f + ci, 29f / 255f + ci));
    }
}
