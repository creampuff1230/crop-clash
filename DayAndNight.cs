using UnityEngine;
using System.Collections;

public class DayAndNight : MonoBehaviour
{
    [Header("Lighting")]
    public Light directionalLight;
    public Color dayAmbientColor = Color.white;
    public Color nightAmbientColor = Color.black;
    public float dayIntensity = 1.0f; // intensity of directional light
    public float nightIntensity = 0.2f; // ^^
    public float transitionSpeed = 5.0f;

    [Header("Skybox")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Fog")]
    public Color dayFogColor = new Color(0.8f, 0.9f, 1.0f);
    public Color nightFogColor = new Color(0.05f, 0.05f, 0.1f);
    public float dayFogDensity = 0.01f;
    public float nightFogDensity = 0.04f;

    [Header("Rotation")]
    public float dayRotationSpeed = 5f;
    public float nightRotationSpeed = 5f;

    // day rotation speed, night rotation speed, and transition speed should all be the same

    private bool isDay = true;
    private float rotationX = 0f;

    void Start()
    {
        // Start the day/night cycle
        StartCoroutine(DayNightCycle());
    }

    private IEnumerator DayNightCycle()
    {
        while (true)
        {
            // slowly transition lighting and fog
            bool transitionComplete = UpdateLightingAndFogGradually(isDay);

            // rotate the directional light
            float rotationSpeed = isDay ? dayRotationSpeed : nightRotationSpeed;
            rotationX += rotationSpeed * Time.deltaTime;
            directionalLight.transform.rotation = Quaternion.Euler(rotationX, 0f, 0f);

            // switch skybox once the transition is complete
            if (transitionComplete)
            {
                RenderSettings.skybox = isDay ? daySkybox : nightSkybox;
                Debug.Log($"Skybox updated to {(isDay ? "Day" : "Night")} Skybox");
            }

            // Check if the day/night should switch
            if (isDay && rotationX >= 180f)
            {
                isDay = false; // switch to night
            }
            else if (!isDay && rotationX >= 360f)
            {
                rotationX = 0f; // reset rotation
                isDay = true;  // switch to day
            }

            yield return null; // wait for the next frame
        }
    }

    private bool UpdateLightingAndFogGradually(bool isDay)
    {
        float tolerance = 0.01f; // threshold for all environment changes for the skybox to change

        // transition ambient light
        Color targetAmbient = isDay ? dayAmbientColor : nightAmbientColor;
        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetAmbient, transitionSpeed * Time.deltaTime);

        // transition directional light intensity
        float targetIntensity = isDay ? dayIntensity : nightIntensity;
        directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, targetIntensity, transitionSpeed * Time.deltaTime);

        // transition fog settings
        Color targetFogColor = isDay ? dayFogColor : nightFogColor;
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, transitionSpeed * Time.deltaTime);

        float targetFogDensity = isDay ? dayFogDensity : nightFogDensity;
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, transitionSpeed * Time.deltaTime);

        // check if the transition is complete with tolerance
        bool ambientDone = CheckTolerance(RenderSettings.ambientLight, targetAmbient, tolerance);
        bool intensityDone = Mathf.Abs(directionalLight.intensity - targetIntensity) < tolerance;
        bool fogColorDone = CheckTolerance(RenderSettings.fogColor, targetFogColor, tolerance);
        bool fogDensityDone = Mathf.Abs(RenderSettings.fogDensity - targetFogDensity) < tolerance;

        // Return true only if all transitions are complete
        return ambientDone && intensityDone && fogColorDone && fogDensityDone;
    }

    // helper function to compare if the colors are within the tolerance
    private bool CheckTolerance(Color a, Color b, float tolerance)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}
