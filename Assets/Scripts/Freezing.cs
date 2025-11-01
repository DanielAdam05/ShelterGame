using System.ComponentModel;
using UnityEngine;

public class Freezing : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField]
    private float freezeMeter = 0f;

    [Space(5)]
    [SerializeField]
    private float FREEZE_SPEED = 0.036f;

    [Space(10)]
    [SerializeField]
    private Transform playerTransform;

    [Header("Collider of area where freezing does not occur")]
    [SerializeField]
    private BoxCollider warmRoomCollider;

    [Space(10)]
    [SerializeField]
    private ParticleSystem coldBreathVFX;

    // Non-assignable variables
    private float currentFreezeTimer;
    private const float FREEZE_INTERVAL = 0.01f;

    private FireManager fireManagerRef;

    private void Awake()
    {
        if (fireManagerRef == null)
        {
            fireManagerRef = gameObject.GetComponent<FireManager>();
        }
    }
    void Start()
    {
        freezeMeter = 0f;
    }

    void Update()
    {
        if (!GameState.IsGameLost())
        {
            if (!GameState.IsGamePaused())
            {
                if (freezeMeter < 100)
                {
                    currentFreezeTimer += Time.deltaTime;
                }

                if (currentFreezeTimer >= FREEZE_INTERVAL)
                {
                    currentFreezeTimer -= FREEZE_INTERVAL;

                    if (!IsPlayerInWarmRoom() || fireManagerRef.FireRanOut()) // freeze
                    {
                        if (coldBreathVFX.isPlaying == false)
                            coldBreathVFX.Play();

                        freezeMeter += FREEZE_SPEED;
                    }
                    else // warm up
                    {
                        if (coldBreathVFX.isPlaying == true)
                            coldBreathVFX.Stop();
               

                        if (freezeMeter > 0)
                        {
                            freezeMeter -= (FREEZE_SPEED * 7);
                        }
                        else if (freezeMeter < 0)
                        {
                            freezeMeter = 0;
                        }
                    }
                }
            }
        }
    }

    private bool IsPlayerInWarmRoom()
    {
        return warmRoomCollider.bounds.Contains(playerTransform.position);
    }

    public float FreezeMeter()
    {
        return freezeMeter;
    }

    public bool FrozeToDeath()
    {
        return freezeMeter >= 100f;
    }
}
