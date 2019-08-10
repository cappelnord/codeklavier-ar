using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLookup : MonoBehaviour
{

    public Material[] materials = new Material[10];

    private static MaterialLookup instance;
    public static MaterialLookup Instance()
    {
        if (instance == null)
        {
            instance = GameObject.Find("MeshGen").GetComponent<MaterialLookup>();
        }

        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material Get(char symbol)
    {
        int index = (int)char.GetNumericValue(symbol);
        return materials[index];
    }
}
