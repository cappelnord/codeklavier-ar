using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class InteriaLittleIntensity : IntensityBehaviour
{
    private Vector3 scale;
    private MeshRenderer meshRenderer;
    private float frequency;

    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        meshRenderer = GetComponent<MeshRenderer>();
        frequency = Random.Range(0.5f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Gen.IntensityHasChanged) {
            float ci = Gen.ColorIntensity;
            meshRenderer.sharedMaterial.SetColor("_Color", new Color(24f / 255f + ci, 113f / 255f + ci, 0f + ci));
        }
        transform.localScale = scale * (1.0f + (Mathf.Sin(Time.time * frequency) * Gen.Intensity * 0.8f));
    }
}
}