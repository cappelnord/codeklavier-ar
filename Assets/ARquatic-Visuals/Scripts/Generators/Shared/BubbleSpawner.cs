using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;

using ARquatic;


namespace ARquatic.Visuals {
public class BubbleSpawner : MonoBehaviour
{
    public GameObject BubblePrefab;
    public float Probability = 0.05f;
    public float ScaleMultiplier = 1f;

    private GeneratorHerd herd;

    public static float OverallScale = 1f;


    // Start is called before the first frame update
    void Start()
    {
        OverallScale = ScaleMultiplier;
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
}
