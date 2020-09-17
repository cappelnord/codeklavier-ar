using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKConsole : MonoBehaviour
{

    public GameObject CodeLabel;
    public float MaxHeight = 1.5f;
    public int MaxChars = 38;

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
        Add(s, true);
    }

    public void Add(string s, bool performLineSplitting = true)
    {
        // should be StringBuilder but it is not performance critical here ...

        if(s.Length >= MaxChars && performLineSplitting)
        {
            string[] rawTokens = s.Split(' ');

            // break up tokens larger than rawTokens
            List<string> tokens = new List<string>();
            for (int i = 0; i < rawTokens.Length; i++)
            {
                string token = rawTokens[i];

                int shortenNextToken = 0;

                while(token.Length > MaxChars - shortenNextToken)
                {
                    tokens.Add(token.Substring(0, MaxChars - shortenNextToken));
                    token = token.Substring(MaxChars - shortenNextToken);
                    shortenNextToken = 2;
                }

                tokens.Add(token);
            }

            // fill the lines; recombine tokens
            string buffer = "";
            foreach(string token in tokens)
            {
                if (buffer.Length + token.Length <= MaxChars)
                {
                    if (buffer.Length > 0)
                    {
                        buffer = buffer + " ";
                    }
                    buffer = buffer + token;
                } else
                {
                    Add(buffer, false);
                    buffer = "… " + token;
                }
            }
            if(buffer.Length > 2)
            {
                Add(buffer, false);
            }
            return;
        }

        // up here the string should fit the line ...

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
