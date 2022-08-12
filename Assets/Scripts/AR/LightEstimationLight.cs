using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

// https://tutorialsforar.com/using-light-estimation-in-ar-using-arkit-and-arcore-with-unity/

[RequireComponent(typeof(Light))]
public class LightEstimationLight : MonoBehaviour
{
    private ARCameraManager arCameraManager;
    private Light mainLight;

    public bool ApplyAmbientSphericalHarmonics = true;
    public bool AbsoluteAverageForDirection = true;

    private int numMainlightDirections = 0;
    private Quaternion currentMainLightDirection;


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

    [HideInInspector]
    public float m_BrightnessMod = 1.0f;

    void Awake()
    {
        mainLight = GetComponent<Light>();

        if (PersistentData.NightMode)
        {
            Destroy(this);
            mainLight.intensity = m_BrightnessMod;
            return;
        }

        mainLight = GetComponent<Light>();
        arCameraManager = FindObjectOfType<ARCameraManager>();

        if (arCameraManager == null)
        {
            Debug.Log("No ARCameraManager component found in scene.");
        }

        if (mainLight == null)
        {
            Debug.Log("No Light object attached to GameObject.");
            if(ApplyAmbientSphericalHarmonics)
            {
                Debug.Log("Only ambient spherical harmonics will be applied!");
            }
        }
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

    // TODO: Check for native color temperature support

    public float ConditionIntensity(float intensity) {
        intensity *= 1.25f;
        
        if(intensity > 0.95f) intensity = 0.95f;
        if(intensity < 0.3f) intensity = 0.3f;

        return intensity * m_BrightnessMod;
    }

    public Color ConditionHDRColor(Color color) {
        float referenceIntensity = mainLight.intensity;
        float maxChannel = 0f;
        if(color.r > maxChannel) maxChannel = color.r;
        if(color.g > maxChannel) maxChannel = color.g;
        if(color.b > maxChannel) maxChannel = color.b;

        float maxAllowedValue = 0.9f / referenceIntensity;
        if(maxChannel > maxAllowedValue) {
            color = color * (maxAllowedValue / maxChannel);
        } else {
            color = color;
        }

        color = color * 0.55f;

        // See that grayscale value is at least 0.5f

        float grey = color.grayscale;
        if(grey < 0.4f) {
            color = color * (0.4f / grey);
        }

        color = color * m_BrightnessMod;

        color.a = 1f;
        return color.gamma;

    }


    void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (ApplyAmbientSphericalHarmonics)
        {
            if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
            {
                sphericalHarmonics = args.lightEstimation.ambientSphericalHarmonics;
                RenderSettings.ambientMode = AmbientMode.Skybox;
                RenderSettings.ambientProbe = sphericalHarmonics.Value;
            }
        }

        if (mainLight == null) return;


        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightness = args.lightEstimation.averageBrightness.Value;
            mainLight.intensity = ConditionIntensity(brightness.Value);
        }
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            colorTemperature = args.lightEstimation.averageColorTemperature.Value;
            // mainLight.colorTemperature = colorTemperature.Value;
            mainLight.color = Mathf.CorrelatedColorTemperatureToRGB(colorTemperature.Value);
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            colorCorrection = args.lightEstimation.colorCorrection.Value;
            mainLight.color = colorCorrection.Value;
        }
        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            mainLightDirection = args.lightEstimation.mainLightDirection;


            if (AbsoluteAverageForDirection)
            {
                if(numMainlightDirections < 2000) numMainlightDirections++;

                if (numMainlightDirections == 1)
                {
                    currentMainLightDirection = Quaternion.LookRotation(mainLightDirection.Value);
                }
                else
                {
                    currentMainLightDirection = Quaternion.Lerp(currentMainLightDirection, Quaternion.LookRotation(mainLightDirection.Value), 1f / numMainlightDirections);
                }
            } else
            {
                currentMainLightDirection = Quaternion.LookRotation(mainLightDirection.Value);
            }

            mainLight.transform.rotation = currentMainLightDirection;

        }

        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            mainLightIntensityLumens = args.lightEstimation.mainLightIntensityLumens;
            mainLight.intensity = ConditionIntensity(args.lightEstimation.averageMainLightBrightness.Value);
        }

        if (args.lightEstimation.mainLightColor.HasValue)
        {
            mainLightColor = args.lightEstimation.mainLightColor;
            mainLight.color = ConditionHDRColor(mainLightColor.Value);
            /*
            #if PLATFORM_ANDROID
                // old code that we should get rid off!
                
                mainLight.color = mainLightColor.Value / Mathf.PI;
                mainLight.color = mainLight.color.gamma;

                // ARCore returns color in HDR format (can be represented as FP16 and have values above 1.0)
                var camera = arCameraManager.GetComponentInParent<Camera>();
                if (camera == null || !camera.allowHDR)
                {
                    Debug.LogWarning($"HDR Rendering is not allowed.  Color values returned could be above the maximum representable value.");
                }
            #endif
            */
        }
    }
}