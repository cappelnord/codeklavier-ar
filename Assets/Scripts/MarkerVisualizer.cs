using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// plenty of duplication with MarkerManager.cs; did not want to create too much coupling here though

public class MarkerVisualizer : MonoBehaviour
{

    public string sourceFolder = "VuforiaNotLoad";
    public string datasetBaseName = "CodeklavierMarkers";

    private Dictionary<string, TransformSpec> markerTransformSpecs;
    private Dictionary<string, ImageTargetSpec> imageTargetSpecs;

    private bool didFinishLoading = false;
    private bool didReceiveEndMarkerConfig = false;
    private bool didVisualizeMarkers = false;

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

        // TODO: Visualize Markers

        didVisualizeMarkers = true;
    }

        
}
