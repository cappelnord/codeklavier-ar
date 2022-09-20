using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {

public class MaterialLookup : MonoBehaviour
{

    public Material[] materials = new Material[10];
    public Color[] colors = new Color[10];

    private static MaterialLookup instance;

    public static MaterialLookup Instance()
    {
        if (instance == null)
        {
            instance = GameObject.Find("MeshGen").GetComponent<MaterialLookup>();
        }

        return instance;
    }

    // TODO: a bit a heavy cast from char to int via double

    public Material Get(char symbol) => materials[(int) char.GetNumericValue(symbol)];
    public Color GetColor(char symbol) => colors[(int) char.GetNumericValue(symbol)];
}
}