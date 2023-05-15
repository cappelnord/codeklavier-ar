using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ARquatic.App {

public class ChannelInfoPopulator : MonoBehaviour
{
    public Transform Content;

    public GameObject ChannelNamePrefab;
    public GameObject ChannelStatusPrefab;
    public GameObject ChannelNameDividerPrefab;
    public GameObject ChannelDatePrefab;
    public GameObject ChannelTimezonePrefab;
    public GameObject ChannelDescriptionPrefab;
    public GameObject ChannelJoinButtonPrefab;
    public GameObject ChannelButtonDividerPrefab;
    public GameObject ChannelMoreInformationButtonPrefab;
    public GameObject ChannelDividerPrefab;

    public GameObject ToARBlack;

    private bool startedTransitionToAR = false;

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
        description.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.MainMenuChannelInformationError;
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

            if(info.visible != true)
            {
                continue;
            }

            GameObject name = Instantiate(ChannelNamePrefab, Content);

            string nameString = info.name;
            if(info.name_nl != "" && ARAppUITexts.IsDutch()) {
                nameString = info.name_nl;
            }

            name.GetComponent<TextMeshProUGUI>().text = nameString;

            GameObject status = Instantiate(ChannelStatusPrefab, Content);

            if(info.status == "online")
            {
                status.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.MainMenuStatusOnline;
                status.GetComponent<TextMeshProUGUI>().color = new Color(0.4f, 1f, 0.4f, 1.0f);
            } else if(info.status == "offline")
            {
                status.GetComponent<TextMeshProUGUI>().text = ARAppUITexts.MainMenuStatusOffline;
                status.GetComponent<TextMeshProUGUI>().color = new Color(1f, 0.4f, 0.4f, 1.0f);
            } else if(info.status == "bundled") {
                status.GetComponent<TextMeshProUGUI>().text = "";
            } else
            {
                status.GetComponent<TextMeshProUGUI>().text = info.status;
            }

            Instantiate(ChannelNameDividerPrefab, Content);

            if(info.eventISODate != "" && info.eventISODate != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(ARAppUITexts.DateDisplayCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                DateTime date = DateTime.Parse(info.eventISODate, null, System.Globalization.DateTimeStyles.RoundtripKind).ToLocalTime();

                string timeZoneString = TimeZoneInfo.Local.StandardName;
                string dateString = date.ToString("g");

                GameObject eventDate = Instantiate(ChannelDatePrefab, Content);

                if (timeZoneString.Length > 6) {
                    eventDate.GetComponent<TextMeshProUGUI>().text = dateString;

                    GameObject timezone = Instantiate(ChannelTimezonePrefab, Content);
                    timezone.GetComponent<TextMeshProUGUI>().text = timeZoneString;
                } else
                {
                    eventDate.GetComponent<TextMeshProUGUI>().text = dateString + " " + timeZoneString;
                }

            }

            GameObject description = Instantiate(ChannelDescriptionPrefab, Content);

            string descriptionString = info.description;
            if(info.description_nl != "" && ARAppUITexts.IsDutch()) {
                descriptionString = info.description_nl;
            }

            description.GetComponent<TextMeshProUGUI>().text = descriptionString;

            bool hadButton = false;
            bool isBundled = info.status == "bundled";

            if(info.status == "online" || isBundled)
            {
                GameObject joinButton = Instantiate(ChannelJoinButtonPrefab, Content);

                if(isBundled) {
                    joinButton.GetComponentInChildren<TextMeshProUGUI>().text = ARAppUITexts.ButtonStartBundled;
                }   else {
                    joinButton.GetComponentInChildren<TextMeshProUGUI>().text = ARAppUITexts.ButtonJoin;
                }

                Button button = joinButton.GetComponent<Button>();

                float baseDistance = info.baseDistance;
                float baseScale = info.baseScale;
                float brightnessMultiplier = info.brightnessMultiplier;
                bool nightMode = info.nightMode;

                button.onClick.AddListener(delegate() {
                    if (startedTransitionToAR) return;
                    startedTransitionToAR = true;

                    PersistentData.SelectedChannel = id;
                    PersistentData.FromMainMenu = true;
                    PersistentData.BaseDistance = baseDistance;
                    PersistentData.BaseScale = baseScale;
                    PersistentData.BrightnessMultiplier = brightnessMultiplier;
                    PersistentData.NightMode = nightMode;

                    PersistentData.IsBundledChannel = isBundled;

                    if(isBundled) {
                        PersistentData.BundledID = info.bundledID;
                    } else {
                        PersistentData.BundledID = null;
                    }

                    ToARBlack.GetComponent<RawImage>().enabled = true;
                    ToARBlack.GetComponent<FadeRawImageAlpha>().Start();
                    ToARBlack.GetComponent<FadeRawImageAlpha>().StartFade(1f, 0.5f, delegate ()
                    {
                        string scene = "TheAR";
                        if(PersistentData.TestbedFakeAR)
                        {
                            scene = "ARTestbed";
                        }
                        SceneManager.LoadScene(scene);
                    });

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
                infoButton.GetComponentInChildren<TextMeshProUGUI>().text = ARAppUITexts.ButtonMoreInformation;

                Button button = infoButton.GetComponent<Button>();

                button.onClick.AddListener(delegate() {
                    Application.OpenURL(info.eventURL);
                });
            }


            Instantiate(ChannelDividerPrefab, Content);
        }

    }
}
}