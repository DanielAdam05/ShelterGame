using System.ComponentModel;
using UnityEngine;

public class Freezing : MonoBehaviour
{
    [Tooltip("Collider of area where freezing does not occur")]
    [SerializeField]
    private BoxCollider warmRoomCollider;

    [Space(10)]
    [SerializeField]
    private Transform playerTransform;

    [Space(10)]
    [SerializeField]
    private float freezeMeter = 0;

    // Non-assignable variables
    private float currentFreezeTimer;
    private const float FREEZE_INTERVAL = 0.01f;

    private const float FREEZE_SPEED = 0.033f;


    void Start()
    {
        freezeMeter = 0;
    }

    void Update()
    {
        if (freezeMeter < 100)
        {
            currentFreezeTimer += Time.deltaTime;
        }

        if (currentFreezeTimer >= FREEZE_INTERVAL)
        {
            currentFreezeTimer -= FREEZE_INTERVAL;

            if (!IsPlayerInWarmRoom()) // freeze
            {
                freezeMeter += FREEZE_SPEED;
            }
            else // warm up
            {
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
