using UnityEngine;

public class DawnLogic : MonoBehaviour
{
    [Header("Dawn Endgame Variables")]
    [SerializeField]
    private float dawnDuration = 60f;
    [SerializeField]
    private Light sunLight;

    // Dawn non-assignable variables
    private float dawnTimer = 0f;
    private const float DAWN_CHANGE_SECONDS = 0.5f;
    private float dawnChangeDivisor;

    private Color targetAmbientColor;
    private Color endGoalAmbientColor = new(0.63f, 0.41f, 0.14f);
    private Color ambientColorStep;
    private float sunLightIntesnityStep;
    private float fogDensityStep;

    void Start()
    {
        dawnChangeDivisor = dawnDuration / DAWN_CHANGE_SECONDS;

        targetAmbientColor = RenderSettings.ambientLight;
        ambientColorStep = (endGoalAmbientColor - targetAmbientColor) / dawnChangeDivisor;
        sunLightIntesnityStep = 2.5f / dawnChangeDivisor;
        fogDensityStep = RenderSettings.fogDensity / dawnChangeDivisor;
    }

    public void UpdateDawn()
    {
        dawnTimer += Time.deltaTime;

        if (dawnTimer >= DAWN_CHANGE_SECONDS)
        {
            dawnTimer -= DAWN_CHANGE_SECONDS;

            targetAmbientColor += ambientColorStep;
            sunLight.intensity += sunLightIntesnityStep;
            RenderSettings.fogDensity -= fogDensityStep;

            if (sunLight.intensity >= 2.5f)
            {
                GameState.SetGameWon();
            }
        }

        RenderSettings.ambientLight = targetAmbientColor;
    }
}
