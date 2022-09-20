using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class PlanetStripesCycler : MonoBehaviour
{

    public Vector4 PhaseFreqeuncy;
    public Vector4 PhaseOffset;

    public Color Color;
    public Color BackgroundColor;

    public float Offset;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }


    // TODO: Can't I reuse the MaterialPropertyBlock?

    void Update()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Phase", Time.time * 0.1f + Offset);
        block.SetVector("_PhaseFrequency", PhaseFreqeuncy);
        block.SetVector("_PhaseCoordOffset", PhaseOffset);
        block.SetColor("_Color", Color);
        block.SetColor("_BackgroundColor", BackgroundColor);
        meshRenderer.SetPropertyBlock(block);
    }
}
}
