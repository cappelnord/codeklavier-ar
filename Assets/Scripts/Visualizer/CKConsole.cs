using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKConsole : MonoBehaviour
{

    public GameObject CodeLabel;
    public float MaxHeight = 1.5f;

    void OnEnable() => EventManager.OnConsole += Add;

    void OnDisable() => EventManager.OnConsole -= Add;

    public void Reset()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Add(string s)
    {
        foreach (Transform child in transform)
        {
            child.localPosition = child.localPosition + new Vector3(0.0f, 0.25f, 0.0f);
            if(child.localPosition.y > MaxHeight)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        GameObject obj = Object.Instantiate(CodeLabel, gameObject.transform);
        obj.GetComponent<TextMesh>().text = s;
    }
}
