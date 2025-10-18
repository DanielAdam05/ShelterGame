using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlankManager : MonoBehaviour
{
    [Header("Worldspace Plank Locations")]
    [SerializeField]
    private List<GameObject> levelPlanks;

    


    [Space(10)]
    [SerializeField]
    private int activatedPlanksOnStart = 10;
    [Space(5)]
    [SerializeField]
    private int carriedPlanks = 5;
    

    // Non-assignable variables

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Carried planks: " + carriedPlanks);
        EnableRandomPlanks(levelPlanks, activatedPlanksOnStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnableRandomPlanks(List<GameObject> container, int count)
    {
        List<GameObject> tempList = new(container);
        int startSize = tempList.Count;

        Debug.Log("Start size: " + startSize);

        for (int i = 0; i < count; ++i) // loop to randomize planks [count] times
        {
            int randomIdx = Random.Range(i, startSize);

            (tempList[randomIdx], tempList[i]) = (tempList[i], tempList[randomIdx]);
        }

        for (int i = 0; i < count; ++i) // activate the planks
        {
            tempList[i].SetActive(true);
        }
    }
}
