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

    private string WaitingForTracking = "Move your device slowly to establish tracking ...";
    private string TapOnCube = "... tap on the cube to place the AR!";

    private TextMeshProUGUI tmp;
    private Image img;


    IEnumerator Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        img = GetComponent<Image>();
        tmp.text = WaitingForTracking;

        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            // Start some fallback experience for unsupported devices
        }
        else
        {
            // Start the AR session
            Session.enabled = true;
        }
    }

    void Update()
    {

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
                NotTracking(WaitingForTracking);
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
                NotTracking(WaitingForTracking);
                break;
            case NotTrackingReason.Relocalizing:
                break;
            case NotTrackingReason.InsufficientLight:
                break;
            case NotTrackingReason.InsufficientFeatures:
                break;
            case NotTrackingReason.ExcessiveMotion:
                NotTracking(WaitingForTracking);
                break;
            case NotTrackingReason.Unsupported:
                break;
        }

        if (ResetPosition.Active && !tmp.enabled)
        {
            tmp.text = TapOnCube;
            tmp.enabled = true;
            img.enabled = true;
        }
    }

    private void Tracking()
    {
        tmp.enabled = false;
        img.enabled = false;
        ResetPosition.Show();
    }

    private void NotTracking(string text)
    {
        tmp.text = text;
        tmp.enabled = true;
        img.enabled = true;
        ResetPosition.Hide();
    }
}