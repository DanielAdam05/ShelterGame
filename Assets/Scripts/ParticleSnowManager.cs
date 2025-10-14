using UnityEngine;

public class ParticleSnowManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem SnowParticleSystem;

    void Start()
    {
        SnowParticleSystem.Play();
    }

    void Update()
    {
        
    }

    public void StartSnow()
    {
        if(!SnowParticleSystem.isPlaying)
            SnowParticleSystem.Play();
    }

    public void StopSnow()
    {
        if (SnowParticleSystem.isPlaying)
            SnowParticleSystem.Stop();
    }
}
