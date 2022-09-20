using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class InteriaOuterIntensity : IntensityBehaviour
{

    private MaterialPropertyBlock block;

    private float offset;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();

        offset = Random.Range(0f, 6f);

        block = new MaterialPropertyBlock();
        block.SetFloat("_Phase", offset);
        block.SetFloat("_Alpha", 0.1f);
        block.SetVector("_PhaseFrequency", new Vector4(Random.Range(2, 5), Random.Range(2, 5), Random.Range(2, 5), Random.Range(2, 5)));
        block.SetVector("_PhaseCoordOffset", new Vector4(Random.Range(4, 10), Random.Range(4, 10), Random.Range(4, 10), Random.Range(4, 10)));

        Color green = Color.HSVToRGB(Random.Range(0.29f, 0.3f), Random.Range(0.8f, 0.9f), Random.Range(0.5f, 0.55f));

        block.SetFloat("_Intensity", Gen.Intensity);
        block.SetFloat("_ColorIntensity", Gen.ColorIntensity);

        block.SetColor("_Color", green);
        block.SetColor("_BackgroundColor", green * 0.6f);

        meshRenderer.SetPropertyBlock(block);
    }

    // Update is called once per frame
    void Update()
    {
        block.SetFloat("_Phase", Time.time * 0.2f);
        block.SetFloat("_Intensity", Gen.Intensity);
        block.SetFloat("_ColorIntensity", Gen.ColorIntensity);

        meshRenderer.SetPropertyBlock(block);
    }
}
}