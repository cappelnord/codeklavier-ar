using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChannelInfoPopulator : MonoBehaviour
{
    public Transform Content;

    public GameObject ChannelNamePrefab;
    public GameObject ChannelStatusPrefab;
    public GameObject ChannelNameDividerPrefab;
    public GameObject ChannelDatePrefab;
    public GameObject ChannelDescriptionPrefab;
    public GameObject ChannelJoinButtonPrefab;
    public GameObject ChannelButtonDividerPrefab;
    public GameObject ChannelMoreInformationButtonPrefab;
    public GameObject ChannelDividerPrefab;

    void Start()
    {
        Clean();
    }

    public void Clean()
    {
        foreach (Transform t in Content)
        {
            Destroy(t.gameObject);
        }
    }

    public void DisplayError()
    {
        GameObject description = Instantiate(ChannelDescriptionPrefab, Content);
        description.GetComponent<TextMeshProUGUI>().text = "Could not refresh channel information ...";
    }

    public void Populate(MasterResponseChannelInfoPair[] channelPairs)
    {
        Clean();

        foreach(MasterResponseChannelInfoPair pair in channelPairs)
        {
            string id = pair.id;
            MasterResponseChannelInfo info = pair.info;

            if(info.name == null)
            {
                Debug.Log("Got empty name in channel info. Broken JSON?");
                continue;
            }

            GameObject name = Instantiate(ChannelNamePrefab, Content);
            name.GetComponent<TextMeshProUGUI>().text = info.name;

            GameObject status = Instantiate(ChannelStatusPrefab, Content);
            status.GetComponent<TextMeshProUGUI>().text = info.status;

            if(info.status == "online")
            {
                status.GetComponent<TextMeshProUGUI>().color = new Color(0.4f, 1f, 0.4f, 1.0f);
            } else if(info.status == "offline")
            {
                status.GetComponent<TextMeshProUGUI>().color = new Color(1f, 0.4f, 0.4f, 1.0f);
            }

            Instantiate(ChannelNameDividerPrefab, Content);

            if(info.eventISODate != "" && info.eventISODate != null)
            {
                GameObject eventDate = Instantiate(ChannelDatePrefab, Content);
                DateTime date = DateTime.Parse(info.eventISODate, null, System.Globalization.DateTimeStyles.RoundtripKind);

                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

                eventDate.GetComponent<TextMeshProUGUI>().text = date.ToString("g") + " UTC";
            }

            GameObject description = Instantiate(ChannelDescriptionPrefab, Content);
            description.GetComponent<TextMeshProUGUI>().text = info.description;

            bool hadButton = false;

            if(info.status == "online")
            {
                GameObject joinButton = Instantiate(ChannelJoinButtonPrefab, Content);
                Button button = joinButton.GetComponent<Button>();

                button.onClick.AddListener(delegate() {
                    Debug.Log(id);
                });

                hadButton = true;
            }

            if(info.eventURL != "" && info.eventURL != null)
            {
                if(hadButton)
                {
                    Instantiate(ChannelButtonDividerPrefab, Content);
                }

                GameObject infoButton = Instantiate(ChannelMoreInformationButtonPrefab, Content);
                Button button = infoButton.GetComponent<Button>();

                button.onClick.AddListener(delegate() {
                    Application.OpenURL(info.eventURL);
                });
            }


            Instantiate(ChannelDividerPrefab, Content);
        }

    }
}
