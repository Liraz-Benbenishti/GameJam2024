using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public VoidEventChannel gameOverEvent;
    public GameObject gameOverPanel;
    public VoidEventChannel winEvent;
    public GameObject WinPanel;

    void showGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void showWinPanel()
    {
        WinPanel.SetActive(true);   
    }

    private void OnEnable()
    {
        gameOverEvent.event_raised += showGameOverPanel;

        winEvent.event_raised += showWinPanel;
    }

    private void OnDisable()
    {
        gameOverEvent.event_raised -= showGameOverPanel;

        winEvent.event_raised -= showWinPanel;

    }
}
