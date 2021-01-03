using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class Loader : MonoBehaviour
{

    public GameObject UnityGray;
    public GameObject LoadingSpinner;
    public GameObject CannotConnectNotification;
    public GameObject AppOutOfDateNotification;
    public GameObject TryAgainButton;
    public GameObject ARNeedsInstall;
    public GameObject ARIsNotSupported;

    private bool arIsAvailable = false;
    private bool isFirstLoad = true;
    private bool isLoading = false;
    private bool success = false;

    private float loadingStartTime;

    void Start()
    {
#if UNITY_IOS
        ARIsNotSupported.GetComponent<TextMeshProUGUI>().text = "Your device is not compatible with ARKit. Unfortunately ARquatic will not run on your device.";
#endif

#if UNITY_ANDROID
        ARIsNotSupported.GetComponent<TextMeshProUGUI>().text = "Your device is not compatible with ARCore. Unfortunately ARquatic will not run on your device.";
#endif

        StartCoroutine(FirstLoad());
    }

    public void TryAgain()
    {
        CannotConnectNotification.SetActive(false);
        TryAgainButton.SetActive(false);
        StartCoroutine(FirstLoad());
    }

    private IEnumerator FirstLoad()
    {
        isFirstLoad = true;

#if !UNITY_EDITOR
        yield return CheckARAvailability();
#else
        Debug.Log("Skipping AR availability check");
        arIsAvailable = true;
#endif

        if(!arIsAvailable)
        {
            yield break;
        }

        loadingStartTime = Time.time;
        isLoading = true;
        yield return LoadMasterInfo();

        isLoading = false;
        LoadingSpinner.SetActive(false);

        if (success)
        {
            isFirstLoad = false;
            UnityGray.GetComponent<FadeRawImageAlpha>().StartFade(0.0f, 2f);
        }
    }

    async void SecondLoad()
    {

    }

    private void NetworkError(string message)
    {
        Debug.Log(message);
        isLoading = false;
        CannotConnectNotification.SetActive(true);
        TryAgainButton.SetActive(true);
        LoadingSpinner.SetActive(false);
    }

    private void CommitData(MasterResponseChannelInfoPair[] channelPairs)
    {

    }

    private IEnumerator InstallAR()
    {

        yield return ARSession.Install();

        if(ARSession.state == ARSessionState.Ready)
        {
            arIsAvailable = true;
        } else
        {
            yield return InstallAR();
        }
    }

    private IEnumerator CheckARAvailability()
    {
        yield return ARSession.CheckAvailability();
        
        switch(ARSession.state)
        {
            case ARSessionState.Unsupported:

                // do not continue, keep unsupported warning visible
                ARIsNotSupported.SetActive(true);
                arIsAvailable = false;
                break;

            case ARSessionState.NeedsInstall:
            case ARSessionState.Installing:

                // recursively call until we have it installed
                ARNeedsInstall.SetActive(true);
                yield return InstallAR();
                ARNeedsInstall.SetActive(false);
                break;

            case ARSessionState.Ready:
            case ARSessionState.SessionInitializing:
            case ARSessionState.SessionTracking:

                // fun times, let's go!
                arIsAvailable = true;
                break;
        }
    }

    private IEnumerator LoadMasterInfo()
    {
        using (UnityWebRequest masterRequest = UnityWebRequest.Get(new Uri($"{MasterServer.BaseURL}app")))
        {
            yield return masterRequest.SendWebRequest();

            if (masterRequest.result != UnityWebRequest.Result.Success)
            {
                NetworkError(masterRequest.error);
                yield break;
            }

            try
            {
                MasterResponseAppInfo response = JsonUtility.FromJson<MasterResponseAppInfo>(masterRequest.downloadHandler.text);

                if(response.protocol > MasterServer.ClientProtocol)
                {
                    AppOutOfDateNotification.SetActive(true);
                    yield break;
                }

                if(response.channelList == null || response.channelList.Length <= 0)
                {
                    NetworkError("No channels available ...");
                    yield break;
                }

                CommitData(response.channelList);

            } catch(Exception e)
            {
                NetworkError(e.Message);
                yield break;
            }

            success = true;
        }
    }

    void Update()
    {
        if(isLoading && loadingStartTime + 0.5f < Time.time)
        {
            LoadingSpinner.SetActive(true);
        }
    }
}
