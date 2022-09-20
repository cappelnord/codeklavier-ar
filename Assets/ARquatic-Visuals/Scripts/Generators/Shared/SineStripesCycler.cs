using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class SineStripesCycler : MonoBehaviour
{
    public float PhaseFrequencyX;
    public float PhaseFrequencyY;
    public float PhaseOffsetX = 0.0f;
    public float PhaseOffsetY = 0.0f;

    public Color Color;
    public Color BackgroundColor;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Phase", Time.time * 0.1f);
        block.SetVector("_PhaseFrequency", new Vector4(PhaseFrequencyX, PhaseFrequencyY, PhaseFrequencyX, PhaseFrequencyY));
        block.SetVector("_PhaseCoordOffset", new Vector4(PhaseOffsetX, PhaseOffsetY, PhaseOffsetX, PhaseOffsetY));
        block.SetColor("_Color", Color);
        block.SetColor("_BackgroundColor", BackgroundColor);
        meshRenderer.SetPropertyBlock(block);
    }
}
}
