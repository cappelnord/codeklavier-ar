using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKConsole : MonoBehaviour
{

    public GameObject codeLabel;
    public float maxHeight = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        EventManager.OnConsole += Add;
    }

    void OnDisable()
    {
        EventManager.OnConsole -= Add;
    }

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
            if(child.localPosition.y > maxHeight)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        GameObject obj = Object.Instantiate(codeLabel, gameObject.transform);
        obj.GetComponent<TextMesh>().text = s;
    }
}
