using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetStripesCycler : MonoBehaviour
{

    public Vector4 phaseFreqeuncy;
    public Vector4 phaseOffset;

    public Color color;
    public Color backgroundColor;

    public float offset;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Phase", Time.time * 0.1f + offset);
        block.SetVector("_PhaseFrequency", phaseFreqeuncy);
        block.SetVector("_PhaseCoordOffset", phaseOffset);
        block.SetColor("_Color", color);
        block.SetColor("_BackgroundColor", backgroundColor);
        gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(block);
    }
}
