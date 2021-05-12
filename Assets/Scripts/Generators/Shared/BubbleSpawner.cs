using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject BubblePrefab;
    public float Probability = 0.05f;

    private GeneratorHerd herd;



    // Start is called before the first frame update
    void Start()
    {
        herd = transform.parent.GetComponent<GeneratorHerd>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(WorldIsBoxed.Status)
        {
            return;
        }

        foreach (string key in herd.Objects.Keys)
        {
            if (Random.Range(0f, 1f) < Probability)
            {
                if (herd.Objects[key].LGen)
                {
                    herd.Objects[key].LGen.SpawnBubble(BubblePrefab);
                }
            }
        }
    }
}
