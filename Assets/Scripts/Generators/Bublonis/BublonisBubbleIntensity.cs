using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BublonisBubbleIntensity : IntensityBehaviour
{
    private Vector3 scale;
    new private MeshRenderer renderer;
    private float frequency;

    private Color green;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        frequency = Random.Range(0.5f, 3f);
        green = new Color(80f / 255f, 198f / 255f , 29f / 255f);

    }

    // Update is called once per frame
    void Update()
    {
        float ci = Gen.ColorIntensity;
        renderer.material.SetColor("_Color", Color.Lerp(green, KeyColor, ci));
    }
}
