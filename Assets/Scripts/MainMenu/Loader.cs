using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{

    public GameObject UnityGray;
    public GameObject LoadingSpinner;
    public GameObject CannotConnectNotification;
    public GameObject AppOutOfDateNotification;
    public GameObject TryAgainButton;

    private bool isFirstLoad = true;
    private bool isLoading = true;
    private bool success = false;

    void Start()
    {
        StartCoroutine(FirstLoad());
    }

    // TODO: Loadng Spinner if things take long - simulate slow network
    // TODO: Implement TryAgain Button


    private IEnumerator FirstLoad()
    {
        isLoading = true;
        isFirstLoad = true;

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
                    isLoading = false;
                    LoadingSpinner.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
