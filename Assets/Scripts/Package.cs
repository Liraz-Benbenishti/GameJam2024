using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public VoidEventChannel gameOverEvent;
    public LayerMask groundLayer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Floor")
        {
            gameOverEvent.raiseEvent();
        }
    }
}
