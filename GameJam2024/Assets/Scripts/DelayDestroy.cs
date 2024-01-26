using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delay = 1.0f; // Delay in seconds before destroying the object

    void Start()
    {
        // Invoke the DestroyObject method after the specified delay
        Invoke("DestroyObject", delay);
    }

    void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}
