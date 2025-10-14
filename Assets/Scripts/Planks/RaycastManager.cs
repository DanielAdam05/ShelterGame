using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastManager : MonoBehaviour
{
    [Space(10)]
    [SerializeField]
    private float rayRange = 2f;

    // Non-assignable variables
    private RaycastHit hitRecord;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayRange, Color.red);
    }

    public bool LookingAtPlank()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitRecord, rayRange))
        {
            if (hitRecord.collider != null)
            {
                if (hitRecord.collider.CompareTag("Plank"))
                {
                    Debug.Log("Looking at plank");
                    
                    return true;
                }
            }
        }
        return false;
    }
}
