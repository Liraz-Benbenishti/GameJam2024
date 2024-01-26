using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public PlayerMovement player;
    public VoidEventChannel gameOverEvent;
    public GameObject gameOverPanel;
    public VoidEventChannel winEvent;
    public GameObject WinPanel;
    public GameObject[] hearts;

    private void Start()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(true);
        }
    }
    void showGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void showWinPanel()
    {
        WinPanel.SetActive(true);   
    }

    private void Update()
    {
        //if (shareHeart.health == 0)
        //{
        //    //heart3.SetActive(false);
        //}
        //if (shareHeart.health == 1)
        //{
        //    //heart2.SetActive(false);
        //}
        //if (shareHeart.health == 2)
        //{
        //    //heart1.SetActive(false);
        //}
    }

    private void OnEnable()
    {
        gameOverEvent.event_raised += showGameOverPanel;
        player.onObsticle += looseHealth;
        winEvent.event_raised += showWinPanel;
    }

    private void OnDisable()
    {
        gameOverEvent.event_raised -= showGameOverPanel;
        player.onObsticle -= looseHealth;
        winEvent.event_raised -= showWinPanel;

    }

    void looseHealth()
    {
        var currentHealth = player.currentHealth;
        hearts[currentHealth].SetActive(false);
    }
}
