using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadMarkerTexture : MonoBehaviour
{
    public string MarkerID;
    public string MarkerFolder = "MarkerTextures";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        string url = Application.streamingAssetsPath + "/" + MarkerFolder + "/" + MarkerID + ".jpg";

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                EventManager.InvokeConsole(uwr.error);
            }
            else
            {
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.mainTexture = DownloadHandlerTexture.GetContent(uwr);

                renderer.material.mainTextureScale = new Vector2(-1.0f, -1.0f);
            }
        }
    }

    public void SetDisplayed(bool what)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = what;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = what;
    }
}
