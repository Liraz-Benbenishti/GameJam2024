using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public float health = 100f;
    public VoidEventChannel gameOverEvent;
    public LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void getHurt(float damage)
    {
        health -= damage;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == 3)
        {
            gameOverEvent.raiseEvent();
        }
    }
}
