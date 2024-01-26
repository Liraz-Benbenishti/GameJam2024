using System;
using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponent<PlayerMovement>();
            
            player.ApplyPowerUp(this);
            StartCoroutine(player.RemovePowerUp(this));
            
            transform.parent.gameObject.SetActive(false);
        }
    }
}
