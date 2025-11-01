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

    // Non-assignable variables
    public bool boardedWindow;

    private AudioSource knockSFX;
    private GameObject shadowCreature;

    private const float FADEOUT_DURATION = 1f;
   

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
