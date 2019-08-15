using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMarkerTexture : MonoBehaviour
{

    public string markerID;
    public string markerFolder = "MarkerTextures";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        string url = Application.streamingAssetsPath + "/" + markerFolder + "/" + markerID + ".jpg";

        using (WWW www = new WWW(url))
        {
            // Wait for download to complete
            yield return www;

            // assign texture
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = www.texture;

            renderer.material.mainTextureScale = new Vector2(-1.0f, -1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
