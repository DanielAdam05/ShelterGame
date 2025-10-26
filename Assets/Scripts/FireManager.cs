using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Analytics;

public class FireManager : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float fireStrength = 1f;
    [Space(5)]
    [SerializeField]
    private float FIRE_INTERVAL = 15f;

    [Header("Fire Objects to Change")]
    [SerializeField]
    private Light fireLight;
    [SerializeField]
    private ParticleSystem fireSystem;

    // Non-assignable variables
    private Vector3 intialParticleSystemScale;
    private const float minScale = 0.3f;
    private const float maxScale = 0.9f;

    private float currentFireTimer;
    
    void Start()
    {
        intialParticleSystemScale = fireSystem.transform.localScale;
        //Debug.Log("Fire scale: " + intialParticleSystemScale);
    }

    private void Update()
    {
        if (!FireRanOut())
        {
            currentFireTimer += Time.deltaTime;

            if (currentFireTimer >= FIRE_INTERVAL)
            {
                currentFireTimer -= FIRE_INTERVAL;
                ChangeFireStrength(-0.2f);
            }
        }
    }

    private void UpdateFireAppearance()
    {
        fireLight.intensity = fireStrength * 100;

        float newScale;
        if (fireStrength <= 0.2f)
        {
            newScale = Mathf.Lerp(0f, minScale, fireStrength / 0.2f);
        }
        else
        {
            float t = (fireStrength - 0.2f) / (1f - 0.2f);
            newScale = Mathf.Lerp(minScale, maxScale, t);
        }

        fireSystem.transform.localScale = intialParticleSystemScale * newScale;
    }

    public void ChangeFireStrength(float change)
    {
        fireStrength += change;
        Debug.Log("Fire strength change is: " + change);

        fireStrength = Mathf.Clamp01(fireStrength);
        Debug.Log("FIRE STRENGTH:  " + fireStrength);
        UpdateFireAppearance();

        currentFireTimer = 0f;
    }

    public bool FireRanOut()
    {
        return fireStrength <= 1e-5f;
    }
}
