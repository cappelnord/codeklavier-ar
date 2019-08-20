using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineStripesCycler : MonoBehaviour
{

    public float phaseFrequencyX;
    public float phaseFrequencyY;
    public float phaseOffsetX = 0.0f;
    public float phaseOffsetY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Phase", Time.time * 0.1f);
        block.SetVector("_PhaseFrequency", new Vector4(phaseFrequencyX, phaseFrequencyY, 0.0f, 0.0f));
        block.SetVector("_PhaseCoordOffset", new Vector4(phaseOffsetX, phaseOffsetY, 0.0f, 0.0f));

        gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(block);
    }
}
