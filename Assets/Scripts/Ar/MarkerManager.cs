using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using Vuforia;

public class MarkerManager : MonoBehaviour
{

    public string sourceFolder;
    public string datasetBaseName;
    public string multiMarkerName = "CodeKlavierMulti";

    private string targetXmlPath;
    private string targetDatPath;
    private string targetDatasetName;

    private string xmlTemplateString;
    private string multiTargetParts = "<Part name=\"Marker-1\" translation=\"0 0 0\" rotation=\"AD: 1 0 0 0\"/>";

    private bool didInitiVuforiaData = false;

    IEnumerator Start()
    {
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
            xmlTemplateString = www.text;

            using (WWW wwwData = new WWW(sourceDatPath))
            {
                yield return wwwData;
                File.WriteAllBytes(targetDatPath, wwwData.bytes);
                InitVuforiaData();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitVuforiaData()
    {
        if (didInitiVuforiaData) return;

        // TODO: Clean up trackables that should not be there
        // multiTargetParts = "";

        string xmlFragment = "<MultiTarget name=\"" + multiMarkerName + "\">" + multiTargetParts + "</MultiTarget>";
        string xmlString = xmlTemplateString.Replace("<!--mt-->", xmlFragment);
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

                    mainObject.SetActive(true);
                }
            }
        }
        didInitiVuforiaData = true;
    }
}
