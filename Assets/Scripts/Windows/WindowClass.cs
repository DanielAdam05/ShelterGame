using UnityEngine;

public class WindowClass : MonoBehaviour
{
    [SerializeField]
    private GameObject boardedPlanks;

    // Non-assignable variables
    public bool boardedWindow;
    
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

    public void BreakWindow()
    {
        boardedWindow = false;
        boardedPlanks.SetActive(false);
    }

    public bool IsWindowBoarded()
    {
        return boardedWindow;
    }
}
