using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.App {

public class MainMenuController : MonoBehaviour
{

    public GameObject Channels;
    public GameObject About;

    void Start()
    {
        DisplayChannels();
    }

    public void DisplayChannels()
    {
        Channels.SetActive(true);
        About.SetActive(false);
    }

    public void DisplayAbout()
    {
        Channels.SetActive(false);
        About.SetActive(true);
    }
}
}