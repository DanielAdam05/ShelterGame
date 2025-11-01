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
        knockSFX.Play();
    }

    public bool IsWindowBoarded()
    {
        return boardedWindow;
    }
}
