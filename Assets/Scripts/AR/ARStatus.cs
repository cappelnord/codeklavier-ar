using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using TMPro;


public class ARStatus : MonoBehaviour
{
    public ARSession Session = null;
    public ResetPosition ResetPosition;

    private TextMeshProUGUI tmp;
    private Image img;

    [HideInInspector]
    public bool StatusVisible = true;


    IEnumerator Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        img = GetComponent<Image>();
        tmp.text = ARAppUITexts.ARStatusWaitingForTracking;

        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            // should not happen!
        }
        else
        {
            // Start the AR session
            Session.enabled = true;
        }
    }

    void Update()
    {

        // most cases should not happen at this point as we test on startup ...

        switch (ARSession.state)
        {
            case ARSessionState.None:

                break;
            case ARSessionState.Unsupported:

                break;
            case ARSessionState.CheckingAvailability:

                break;
            case ARSessionState.NeedsInstall:

                break;
            case ARSessionState.Installing:

                break;
            case ARSessionState.Ready:

                break;
            case ARSessionState.SessionInitializing:
                NotTracking(ARAppUITexts.ARStatusWaitingForTracking);
                break;
            case ARSessionState.SessionTracking:
                Tracking();
                break;
        }

        switch (ARSession.notTrackingReason)
        {
            case NotTrackingReason.None:
                break;
            case NotTrackingReason.Initializing:
                NotTracking(ARAppUITexts.ARStatusWaitingForTracking);
                break;
            case NotTrackingReason.Relocalizing:
                break;
            case NotTrackingReason.InsufficientLight:
                NotTracking(ARAppUITexts.ARStatusInsufficientLight);
                break;
            case NotTrackingReason.InsufficientFeatures:
                NotTracking(ARAppUITexts.ARStatusInsufficientFeatures);
                break;
            case NotTrackingReason.ExcessiveMotion:
                NotTracking(ARAppUITexts.ARStatusWaitingForTracking);
                break;
            case NotTrackingReason.Unsupported:
                break;
        }

        if (ResetPosition.Active && !tmp.enabled)
        {
            tmp.text = ARAppUITexts.ARStatusTapOnCube;
            tmp.enabled = true;
            img.enabled = true;
        }
    }

    private void Tracking()
    {
        tmp.enabled = false;
        img.enabled = false;
        StatusVisible = false;
        ResetPosition.Show();
    }

    private void NotTracking(string text)
    {
        tmp.text = text;
        tmp.enabled = true;
        img.enabled = true;
        StatusVisible = true;
        ResetPosition.Hide();
    }
}