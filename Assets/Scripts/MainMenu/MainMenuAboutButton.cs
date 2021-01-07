using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;


public class MainMenuAboutButton : MonoBehaviour, IPointerClickHandler
{
    public MainMenuController Controller;

    public void OnPointerClick(PointerEventData eventData)
    {
        Controller.DisplayAbout();
    }
}