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
    public GameObject NoARContinueButton;
    public GameObject ARNeedsInstall;
    public GameObject ARIsNotSupported;
    public GameObject VersionDisplay;

    public GameObject RefreshButton;
    public GameObject RefreshSpinner;

    public string AdditionalChannel;

    public bool TriggerLocalNetwork = false;

    private bool arIsAvailable = false;
    private bool isLoading = false;
    private bool success = false;

    private float loadingStartTime;

    private ChannelInfoPopulator populator;

    void Start()
    {
        // see that we start with a fresh AR session
        LoaderUtility.Deinitialize();
        LoaderUtility.Initialize();

        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        if (TriggerLocalNetwork)
        {
            IOSNetworkPermission.TriggerDialog();
        }

        CannotConnectNotification.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.PreMenuCannotConnect;
        AppOutOfDateNotification.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.PreMenuAppOutOfDate;
        TryAgainButton.GetComponentInChildren<TextMeshProUGUI>().text = ARAppUITexts.ButtonTryAgain;
        NoARContinueButton.GetComponentInChildren<TextMeshProUGUI>().text = ARAppUITexts.ButtonNoARContinue;
        ARNeedsInstall.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.PreMenuARNeedsInstall;

        VersionDisplay.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.VersionString;
        populator = GetComponent<ChannelInfoPopulator>();

#if UNITY_IOS
        ARIsNotSupported.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.PreMenuNotCompatibleApple;
#endif

#if UNITY_ANDROID
        ARIsNotSupported.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.PreMenuNotCompatibleAndroid;
#endif

        StartCoroutine(FirstLoad());
    }

    public void TryAgain()
    {
        CannotConnectNotification.SetActive(false);
        TryAgainButton.SetActive(false);
        StartCoroutine(FirstLoad());
    }

    public void NoARContinue()
    {
        // this is a lie!
        arIsAvailable = true;
        PersistentData.TestbedFakeAR = true;
        ARIsNotSupported.SetActive(false);
        NoARContinueButton.SetActive(false);
        StartCoroutine(FirstLoad());
    }

    private IEnumerator FirstLoad()
    {

#if !UNITY_EDITOR
        if(!arIsAvailable) {
           yield return CheckARAvailability();
        }
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
            UnityGray.GetComponent<FadeRawImageAlpha>().StartFade(0.0f, 1f);
        }
    }

    public void StartSecondLoad()
    {
        StartCoroutine(SecondLoad());
    }

    private IEnumerator SecondLoad()
    {
        RefreshSpinner.SetActive(true);
        RefreshButton.SetActive(false);
        populator.Clean();

        success = false;
        yield return LoadMasterInfo();

        if(!success)
        {
            populator.DisplayError();
        }

        RefreshSpinner.SetActive(false);
        RefreshButton.SetActive(true);

        yield break;
    }

    private void NetworkError(string message)
    {
        Debug.Log(message);
        isLoading = false;
        CannotConnectNotification.SetActive(true);
        TryAgainButton.SetActive(true);
        LoadingSpinner.SetActive(false);
    }

    private void Populate(MasterResponseChannelInfoPair[] channelPairs)
    {
        populator.Populate(channelPairs);
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

        switch (ARSession.state)
        {
            case ARSessionState.Unsupported:

                // do not continue, keep unsupported warning visible
                if (!PersistentData.TestbedFakeAR) {
                    ARIsNotSupported.SetActive(true);
                    NoARContinueButton.SetActive(true);
                    arIsAvailable = false;
                } else
                {
                    NoARContinue();
                }
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

    private string MasterServerURL() {
        string add = "";
        if(AdditionalChannel != null && AdditionalChannel != "")
        {
            add = $"?additionalChannel={AdditionalChannel}";
        }

        return $"{MasterServer.BaseURL}app{add}";
    }

    private IEnumerator LoadMasterInfo()
    {
        using (UnityWebRequest masterRequest = UnityWebRequest.Get(new Uri(MasterServerURL())))
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

                Populate(response.channelList);

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
        if(isLoading && loadingStartTime + 1f < Time.time)
        {
            LoadingSpinner.SetActive(true);
        }
    }
}
