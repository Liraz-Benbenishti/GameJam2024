using System;
using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponent<PlayerMovement>();
            
            player.ApplyPowerUp(this);
            //player.RemovePowerUp(this);
            
            gameObject.SetActive(false);
        }
    }
}
