using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField]
    private float rayRange = 2f;
    [SerializeField]
    private LayerMask interactableLayers;

    // Non-assignable variables
    private RaycastHit hitRecord;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayRange, Color.red);
    }

    //public T LookingAtComponent<T>() where T : Component // template function
    //{
    //    if (Physics.Raycast(transform.position, transform.forward, out hitRecord, rayRange, interactableLayers))
    //    {
    //        return hitRecord.collider?.GetComponent<T>();
    //    }
    //    return null;
    //}

    public bool LookingAtTag(string tag)
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitRecord, rayRange, interactableLayers))
        {
            return hitRecord.collider != null && hitRecord.collider.CompareTag(tag);
        }
        return false;
    }

    public RaycastHit GetHitRecord()
    {
        return hitRecord;
    }
}
