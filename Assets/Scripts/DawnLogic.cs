using UnityEngine;

public class DawnLogic : MonoBehaviour
{
    [Header("Dawn Endgame Variables")]
    [SerializeField]
    private float dawnStart = 150f;
    [SerializeField]
    private float dawnDuration = 60f;

    [Header("Objects changed by Dawn")]
    [SerializeField]
    private Light sunLight;
    [SerializeField]
    private Camera renderCamera;

    // Dawn non-assignable variables
    private float dawnTimer = 0f;
    private const float DAWN_CHANGE_SECONDS = 0.05f;
    private float dawnChangeDivisor;

    private Color targetAmbientColor;
    private Color endGoalAmbientColor = new(0.63f, 0.41f, 0.14f);
    private Color ambientColorStep;
    private float sunLightIntesnityStep;
    private float fogDensityStep;
    private Color targetCameraBackgroundColor;
    private Color endGoalCameraBackgroundColor = new(0.64f, 0.5f, 0.24f);
    private Color cameraBackgroundColorStep;

    void Start()
    {
        dawnChangeDivisor = dawnDuration / DAWN_CHANGE_SECONDS;

        targetAmbientColor = RenderSettings.ambientLight;
        ambientColorStep = (endGoalAmbientColor - targetAmbientColor) / dawnChangeDivisor;
        sunLightIntesnityStep = 2.5f / dawnChangeDivisor;
        fogDensityStep = RenderSettings.fogDensity / dawnChangeDivisor;
        targetCameraBackgroundColor = renderCamera.backgroundColor;
        cameraBackgroundColorStep = (endGoalCameraBackgroundColor - targetCameraBackgroundColor) / dawnChangeDivisor;
    }

    public void UpdateDawn()
    {
        if (!GameState.IsGamePaused() && !GameState.IsGameWon() && !GameState.IsGameLost())
        {
            dawnTimer += Time.deltaTime;

            if (dawnTimer >= DAWN_CHANGE_SECONDS)
            {
                dawnTimer -= DAWN_CHANGE_SECONDS;

                targetAmbientColor += ambientColorStep;
                sunLight.intensity += sunLightIntesnityStep;
                RenderSettings.fogDensity -= fogDensityStep;
                targetCameraBackgroundColor += cameraBackgroundColorStep;

                if (sunLight.intensity >= 2.5f)
                {
                    GameState.SetGameWon();
                }
            }

            RenderSettings.ambientLight = targetAmbientColor;
            renderCamera.backgroundColor = targetCameraBackgroundColor;
        }
            
    }

    public float GetDawnStart()
    {
        return dawnStart;
    }
}
