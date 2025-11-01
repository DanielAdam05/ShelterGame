using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [Header("List of all Windows")]
    [SerializeField]
    private List<GameObject> windows = new();

    [Header("Knock Cooldown")]
    [SerializeField]
    private float minCooldown = 15f;
    [SerializeField]
    private float maxCooldown = 30f;
    [SerializeField]
    private float initalCooldown = 45f;

    [Space(10)]
    [SerializeField]
    private float currentKnockCooldown;

    // Non-assignale variables
    private int windowListSize;
    private readonly List<WindowClass> unboardedWindows = new();
    private int unboardedWindowsListSize;

    private float knockingTimer;

    private int randomWindowIdx;


    private void Awake()
    {
        windowListSize = windows.Count;

        unboardedWindows.Capacity = windowListSize;
        //Debug.Log("Inital windows list count: " + windowListSize);

        for (int i = 0; i < windowListSize; ++i)
        {
            unboardedWindows.Add(windows[i].GetComponent<WindowClass>());
        }

        // At start all windows are unboarded
        unboardedWindowsListSize = unboardedWindows.Count;

        //Debug.Log("Unboarded windows list count: " + unboardedWindowsListSize);
    }

    void Start()
    {
        currentKnockCooldown = initalCooldown;
    }

    void Update()
    {
        if (!GameState.IsGameLost() && !GameState.IsGamePaused())
        {
            knockingTimer += Time.deltaTime;

            if (knockingTimer >= currentKnockCooldown)
            {
                knockingTimer -= currentKnockCooldown;

                // Choose random window
                randomWindowIdx = Random.Range(0, unboardedWindowsListSize - 1);
                unboardedWindows[randomWindowIdx].KnockOnWindow();
                Debug.Log("Knocked on window " + randomWindowIdx);

                // Change random cooldown
                currentKnockCooldown = Random.Range(minCooldown, maxCooldown); 
            }
        }
    }

    public void UpdateBoardedWindows()
    {
        for (int i = 0; i < unboardedWindowsListSize; ++i)
        {
            if (unboardedWindows[i].IsWindowBoarded())
            {
                Debug.Log("Window [" + i + "] is boarded!");
                unboardedWindows.Remove(unboardedWindows[i]);
                unboardedWindowsListSize--;
                break;
            }
            //else
            //    Debug.Log(i + " not boarded");
        }
        Debug.Log("Updated Unboarded windows list size: " + unboardedWindowsListSize);
    }
}
