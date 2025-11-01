using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowClass : MonoBehaviour
{
    [Header("Window Members")]
    [SerializeField]
    private GameObject boardedPlanks;

    // Non-assignable variables
    public bool boardedWindow;

    private AudioSource knockSFX;

    private void Awake()
    {
        knockSFX = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
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
    }

    //public void BreakWindow()
    //{
    //    boardedWindow = false;
    //    boardedPlanks.SetActive(false);
    //}

    public void KnockOnWindow()
    {
        knockSFX.enabled = true;
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
}
