using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// plenty of duplication with MarkerManager.cs; did not want to create too much coupling here though

public class MarkerVisualizer : MonoBehaviour
{

    public string sourceFolder = "VuforiaNotLoad";
    public string datasetBaseName = "CodeklavierMarkers";
    public GameObject markerVisualizerPrefab;

    private Dictionary<string, TransformSpec> markerTransformSpecs;
    private Dictionary<string, ImageTargetSpec> imageTargetSpecs;

    private bool didFinishLoading = false;
    private bool didReceiveEndMarkerConfig = false;
    private bool didVisualizeMarkers = false;

    public bool worldIsAr = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        markerTransformSpecs = new Dictionary<string, TransformSpec>();
        string sourceXmlPath = Application.streamingAssetsPath + "/" + sourceFolder + "/" + datasetBaseName + ".xml";

        using (WWW www = new WWW(sourceXmlPath))
        {
            yield return www;
            imageTargetSpecs = MarkerManager.ParseMarkerXml(www.text);

            didFinishLoading = true;
            TryVisualizeMarkers();
        }
    }

    void OnEnable()
    {
        EventManager.OnServerEventEndMarkerConfig += OnServerEventEndMarkerConfig;
        EventManager.OnMarkerTransform += OnMarkerTransform;
    }

    void OnDisable()
    {
        EventManager.OnServerEventEndMarkerConfig -= OnServerEventEndMarkerConfig;
        EventManager.OnMarkerTransform -= OnMarkerTransform;
    }

    public void OnMarkerTransform(string key, TransformSpec ts)
    {
        string markerName = key.Replace("marker-", "");
        markerTransformSpecs[markerName] = ts;
    }

    public void OnServerEventEndMarkerConfig()
    {
        didReceiveEndMarkerConfig = true;
        TryVisualizeMarkers();
    }

    void TryVisualizeMarkers()
    {
        if (!didFinishLoading || !didReceiveEndMarkerConfig) return;
        if (didVisualizeMarkers) return;

        foreach(string key in markerTransformSpecs.Keys)
        {
            if(!imageTargetSpecs.ContainsKey(key))
            {
                Debug.Log("Could not find ImageTargetSpec for:" + key);
                continue;
            }

            TransformSpec ts = markerTransformSpecs[key];
            ImageTargetSpec its = imageTargetSpecs[key];

            GameObject obj = Instantiate(markerVisualizerPrefab, gameObject.transform);

            obj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);


            obj.transform.localEulerAngles = new Vector3(ts.rotation[0], ts.rotation[1], ts.rotation[2]);

            if (!worldIsAr)
            {
                obj.transform.Translate(new Vector3(ts.position[0], ts.position[1], ts.position[2]), Space.World);
            }
            else
            {   // rotated around x; y and z should switch
                obj.transform.Translate(new Vector3(ts.position[0], ts.position[2] * -1.0f, ts.position[1]), Space.World);
            }

            obj.transform.localScale = new Vector3(its.width * ts.scale[0], its.height * ts.scale[1], 0.00001f * ts.scale[2]);

            obj.GetComponent<LoadMarkerTexture>().markerID = key;
        }

        didVisualizeMarkers = true;
    }

        
}
