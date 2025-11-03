using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowClass : MonoBehaviour
{
    [Header("Window Members")]
    [SerializeField]
    private GameObject boardedPlanks;

    [SerializeField]
    private Material shadowCreatureMaterial;

    [Header("Player Camera")]
    [SerializeField]
    private Camera playerCamera;
    [Header("Render Texture for screen size!")]
    [Tooltip("Screen size scales with the render texture")]
    [SerializeField]
    private RenderTexture gameplayRenderTexture;

    [Header("Enemy Raycast Layermask")]
    [SerializeField]
    private LayerMask lineOfSightLayerMask;

    [Space(10)]
    // Non-assignable variables
    public bool boardedWindow;

    private AudioSource knockSFX;
    private GameObject shadowCreature;
    private Vector3 offsetShadowWorldPositon;

    //private const float FADEOUT_DURATION = 1f;

    private Vector3 shadowScreenPos;
    [SerializeField]
    private bool lineOfSightClear;

    private int screenWidth;
    private int screenHeight;
    

    private void Awake()
    {
        knockSFX = gameObject.GetComponent<AudioSource>();
        shadowCreature = gameObject.transform.Find("Shadow").gameObject;

        if (shadowCreature == null)
        {
            Debug.LogError("Couldn't find Shadow child in Window gameobject");
        }
    }

    void Start()
    {
        shadowCreature.SetActive(false);
        boardedWindow = false;

        offsetShadowWorldPositon = shadowCreature.transform.position + new Vector3(0f, 0.4f, 0f);
        shadowScreenPos = playerCamera.WorldToScreenPoint(offsetShadowWorldPositon);

        screenWidth = gameplayRenderTexture.width;
        screenHeight = gameplayRenderTexture.height;
        Debug.Log("Screen size: [" + screenWidth + ", " + screenHeight + "]");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.IsGameLost() && !GameState.IsGamePaused())
        {
            if (shadowCreature.activeSelf)
            {
                offsetShadowWorldPositon = shadowCreature.transform.position + new Vector3(0f, 0.4f, 0f);
                shadowScreenPos = playerCamera.WorldToScreenPoint(offsetShadowWorldPositon);

                //Debug.Log(shadowScreenPos);

                if (EnemyOnScreen(screenWidth, screenHeight))
                {
                    Debug.Log("Enemy on screen!");
                }
            }
        }
    }

    private bool EnemyOnScreen(float screenWidth, float screenHeight)
    {
        Vector3 enemyToPlayerVector = offsetShadowWorldPositon - playerCamera.transform.position;

        if (Physics.Raycast(playerCamera.transform.position, enemyToPlayerVector.normalized, out RaycastHit hit, enemyToPlayerVector.magnitude, lineOfSightLayerMask))
        {
            lineOfSightClear = false;
        }
        else
        {
            lineOfSightClear = true;
        }

        return (shadowScreenPos.x > 0 && shadowScreenPos.x <= screenWidth) &&
                (shadowScreenPos.y > 0 && shadowScreenPos.y <= screenHeight) &&
                shadowScreenPos.z >= 0 &&
                lineOfSightClear;
    }

    public void BoardWindow()
    {
        boardedWindow = true;
        boardedPlanks.SetActive(true);

        shadowCreature.SetActive(false);
    }

    //public void BreakWindow()
    //{
    //    boardedWindow = false;
    //    boardedPlanks.SetActive(false);
    //}

    public void KnockOnWindow()
    {
        knockSFX.enabled = true;
        shadowCreature.SetActive(true);
        //knockSFX.Play();
        StartCoroutine(DisableSoundAfterPlaying());
    }

    public bool IsWindowBoarded()
    {
        return boardedWindow;
    }

    private IEnumerator DisableSoundAfterPlaying()
    {
        //Debug.Log("Knock SFX clip length is: " + knockSFX.clip.length);
        yield return new WaitForSeconds(knockSFX.clip.length);
        knockSFX.enabled = false;
        //Debug.Log("Disabled sound");
    }

    //private IEnumerator FadeShadowOut()
    //{
    //    Color color = shadowCreatureMaterial.color;
    //    float startAlpha = color.a;

    //    float duration = 1f; // 1 second
    //    float elapsedSec = 0f;

    //    while (elapsedSec < duration)
    //    {
    //        elapsedSec += Time.deltaTime;
    //        float alpha = Mathf.Lerp(startAlpha, 0f, elapsedSec / duration);
    //        color.a = alpha;
    //        shadowCreatureMaterial.color = color;
    //        yield return null; // wait for next frame
    //    }

    //    // Ensure alpha is set to 0 at the end
    //    color.a = 0f;
    //    shadowCreatureMaterial.color = color;
    //}
}
