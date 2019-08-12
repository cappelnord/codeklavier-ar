using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Globalization;
using UnityEngine;

using Vuforia;

public class ImageTargetSpec
{
    public string name;
    public float width;
    public float height;

    public ImageTargetSpec(string _name, float _width, float _height)
    {
        name = _name;
        width = _width;
        height = _height;
    }
}

public class MarkerManager : MonoBehaviour
{
    private string xmlTemplateHead = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><QCARConfig xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"qcar_config.xsd\"><Tracking>";
    private string xmlTemplateFoot = "</Tracking></QCARConfig>";

    public string sourceFolder = "VuforiaNotLoad";
    public string datasetBaseName = "CodeklavierMarkers";

    private string multiMarkerName = "CodeKlavierMulti";

    private string targetXmlPath;
    private string targetDatPath;
    private string targetDatasetName;

    private string xmlTemplateString;

    private bool didFinishLoading = false;
    private bool didReceiveEndMarkerConfig = false;
    private bool didInitiVuforiaData = false;

    private Dictionary<string, TransformSpec> markerTransformSpecs;
    private Dictionary<string, ImageTargetSpec> imageTargetSpecs;

    IEnumerator Start()
    {
        markerTransformSpecs = new Dictionary<string, TransformSpec>();

        string sourceXmlPath = Application.streamingAssetsPath + "/" + sourceFolder + "/" + datasetBaseName + ".xml";
        string sourceDatPath = Application.streamingAssetsPath + "/" + sourceFolder + "/" + datasetBaseName + ".dat";
        targetXmlPath = Application.temporaryCachePath + "/" + datasetBaseName + ".xml";
        targetDatPath = Application.temporaryCachePath + "/" + datasetBaseName + ".dat";
        targetDatasetName = Application.temporaryCachePath + "/" + datasetBaseName + ".xml";

        if (File.Exists(targetXmlPath))
        {
            File.Delete(targetXmlPath);
        }
        if(File.Exists(targetDatPath))
        {
            File.Delete(targetDatPath);
        }

        using (WWW www = new WWW(sourceXmlPath))
        {
            yield return www;
            imageTargetSpecs = ParseMarkerXml(www.text);

            using (WWW wwwData = new WWW(sourceDatPath))
            {
                yield return wwwData;
                File.WriteAllBytes(targetDatPath, wwwData.bytes);
                didFinishLoading = true;
                TryInitVuforiaData();
            }
        }
    }

    public static Dictionary<string, ImageTargetSpec> ParseMarkerXml(string xml)
    {
        Dictionary<string, ImageTargetSpec> ret = new Dictionary<string, ImageTargetSpec>();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        XmlNodeList nodeList = doc.SelectNodes(".//" + "ImageTarget");

        foreach(XmlNode node in nodeList)
        {
            string name = node.Attributes["name"].Value;
            string sizeString = node.Attributes["size"].Value;
            string[] sizeToken = sizeString.Split(' ');
            ret[name] = new ImageTargetSpec(name, float.Parse(sizeToken[0], CultureInfo.InvariantCulture), float.Parse(sizeToken[1], CultureInfo.InvariantCulture));
        }

        return ret;
    }

    // Update is called once per frame
    void Update()
    {

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
        TryInitVuforiaData();
    }

    public void TryInitVuforiaData()
    {
        if (!didFinishLoading || !didReceiveEndMarkerConfig) return;
        if (didInitiVuforiaData) return;

        string imageTargetsXml = "";

        foreach (string key in imageTargetSpecs.Keys)
        {
            if(markerTransformSpecs.ContainsKey(key))
            {
                ImageTargetSpec spec = imageTargetSpecs[key];
                string sw = spec.width.ToString("0.000000", CultureInfo.CreateSpecificCulture("en-US"));
                string sh = spec.height.ToString("0.000000", CultureInfo.CreateSpecificCulture("en-US"));
                imageTargetsXml += "<ImageTarget name=\"" + key + "\" size=\"" + sw + " " + sh + "\"/>\n";
            }
        }

        string multiTargetParts = "";

        foreach (string key in markerTransformSpecs.Keys)
        {
            TransformSpec ts = markerTransformSpecs[key];
            multiTargetParts += "<Part name=\"" + key + 
                                "\" translation=\"" + ts.position[0] + " " + ts.position[1] + " " + ts.position[2] + 
                                "\" rotation=\"AD: 1 " + ts.rotation[0] + " " + ts.rotation[1] + " " + ts.rotation[2] +
                                "\"/>\n";
        }


        string multiTargetXml = "<MultiTarget name=\"" + multiMarkerName + "\">" + multiTargetParts + "</MultiTarget>";
        string xmlString = xmlTemplateHead + imageTargetsXml + multiTargetXml + xmlTemplateFoot;
        File.WriteAllText(targetXmlPath, xmlString);
        Debug.Log(targetXmlPath);


        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        DataSet dataSet = objectTracker.CreateDataSet();


        if (dataSet.Load(targetDatasetName, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
        {

            objectTracker.Stop();
            objectTracker.ActivateDataSet(dataSet);
            objectTracker.Start();

            IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
            foreach (TrackableBehaviour tb in tbs)
            {
                tb.gameObject.name = "DynamicImageTarget-" + tb.TrackableName;
                if(tb.TrackableName != multiMarkerName)
                {
                    tb.gameObject.SetActive(false);
                } else
                {
                    // add additional script components for trackable
                    tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
                    // tb.gameObject.AddComponent<TurnOffBehaviour>();
                }
            }
        }
        didInitiVuforiaData = true;
    }
}
