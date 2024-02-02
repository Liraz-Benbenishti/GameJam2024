using UnityEngine;

public class Package : MonoBehaviour
{
    public VoidEventChannel gameOverEvent;
    public LayerMask groundLayer;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == 3)
        {
            gameOverEvent.raiseEvent();
        }
    }
}
