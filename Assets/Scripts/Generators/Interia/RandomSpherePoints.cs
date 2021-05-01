using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpherePoints : MonoBehaviour
{

    public float Radius = 0.5f;
    public float Number = 64;
    public GameObject Prefab;

    public float MinScale = 0.01f;
    public float MaxScale = 0.05f;

    public LGenerator Gen;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // https://www.bogotobogo.com/Algorithms/uniform_distribution_sphere.php#:~:text=To%20distribute%20points%20such%20that,on%20%5B0%2C1%5D.
    private void Generate()
    {
        // not linked to RNG System; probably needs to be injected.

        for(int i = 0; i < Number; i++)
        {
            float theta = 2.0f * Mathf.PI * Random.Range(0.0f, 1.0f);
            float phi = Mathf.Acos(2.0f * Random.Range(0.0f, 1.0f) - 1.0f);

            GameObject obj = Instantiate(Prefab, transform);
            obj.transform.localPosition = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi)) * Radius;

            float scale = Random.Range(MinScale, MaxScale);
            obj.transform.localScale = new Vector3(scale, scale, scale);

            obj.GetComponent<InteriaLittleIntensity>().Gen = Gen;

            // TODO: Combine Mesh?
        }
    }
}
