// https://tutorialsforar.com/using-light-estimation-in-ar-using-arkit-and-arcore-with-unity/

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;

    Light mainLight;

    /// <summary>
    /// The estimated brightness of the physical environment, if available.
    /// </summary>
    public float? brightness { get; private set; }

    /// <summary>
    /// The estimated color temperature of the physical environment, if available.
    /// </summary>
    public float? colorTemperature { get; private set; }

    /// <summary>
    /// The estimated color correction value of the physical environment, if available.
    /// </summary>
    public Color? colorCorrection { get; private set; }

    /// <summary>
    /// The estimated direction of the main light of the physical environment, if available.
    /// </summary>
    public Vector3? mainLightDirection { get; private set; }

    /// <summary>
    /// The estimated color of the main light of the physical environment, if available.
    /// </summary>
    public Color? mainLightColor { get; private set; }

    /// <summary>
    /// The estimated intensity in lumens of main light of the physical environment, if available.
    /// </summary>
    public float? mainLightIntensityLumens { get; private set; }

    /// <summary>
    /// The estimated spherical harmonics coefficients of the physical environment, if available.
    /// </summary>
    public SphericalHarmonicsL2? sphericalHarmonics { get; private set; }

    void Awake()
    {
        mainLight = GetComponent<Light>();
    }

    void OnEnable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived += FrameChanged;
    }

    void OnDisable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived -= FrameChanged;
    }


    void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightness = args.lightEstimation.averageBrightness.Value;
            mainLight.intensity = brightness.Value;
        }
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            colorTemperature = args.lightEstimation.averageColorTemperature.Value;
            mainLight.colorTemperature = colorTemperature.Value;
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            colorCorrection = args.lightEstimation.colorCorrection.Value;
            mainLight.color = colorCorrection.Value;
        }
        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            mainLightDirection = args.lightEstimation.mainLightDirection;
            mainLight.transform.rotation = Quaternion.LookRotation(mainLightDirection.Value);
        }
        if (args.lightEstimation.mainLightColor.HasValue)
        {
            mainLightColor = args.lightEstimation.mainLightColor;

#if PLATFORM_ANDROID
            // ARCore needs to apply energy conservation term (1 / PI) and be placed in gamma
            mainLight.color = mainLightColor.Value / Mathf.PI;
            mainLight.color = mainLight.color.gamma;
            
            // ARCore returns color in HDR format (can be represented as FP16 and have values above 1.0)
            var camera = mainLight.GetComponentInParent<Camera>();
            if (camera == null || !camera.allowHDR)
            {
                Debug.LogWarning($"HDR Rendering is not allowed.  Color values returned could be above the maximum representable value.");
            }
#endif
        }
        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            mainLightIntensityLumens = args.lightEstimation.mainLightIntensityLumens;
            mainLight.intensity = args.lightEstimation.averageMainLightBrightness.Value;
        }
        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            sphericalHarmonics = args.lightEstimation.ambientSphericalHarmonics;
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = sphericalHarmonics.Value;
        }
    }

}