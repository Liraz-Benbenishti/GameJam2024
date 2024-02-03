using System;
using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float duration;
    public PlayerMovement player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.ApplyPowerUp(this);
            backToNormal();
            gameObject.SetActive(false);
        }
    }

    private void backToNormal()
    {
        _ = player.RemovePowerUp(this);
    }
}
