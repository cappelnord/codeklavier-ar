using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARStatus : MonoBehaviour
{
    public ARSession Session = null;
    public ResetPosition ResetPosition;

    public Sprite WaitingForTracking;
    public Sprite TapOnCube;

    private Image image;


    IEnumerator Start()
    {
        image = GetComponent<Image>();

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

        if (ResetPosition.Active && !image.enabled)
        {
            image.sprite = TapOnCube;
            image.enabled = true;
        }
    }

    private void Tracking()
    {
        image.enabled = false;
        ResetPosition.Show();
    }

    private void NotTracking(Sprite sprite)
    {
        image.sprite = sprite;
        image.enabled = true;
        ResetPosition.Hide();
    }
}