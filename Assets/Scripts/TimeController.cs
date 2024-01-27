using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Range(0, 1)]
    public float timeScale = 1;

    public VoidEventChannel winEvent;
    public VoidEventChannel gameOverEvent;

    private void OnEnable()
    {
        winEvent.event_raised += StopGame;
        gameOverEvent.event_raised += StopGame;
    }

    private void OnDisable()
    {
        winEvent.event_raised += StopGame;
        gameOverEvent.event_raised += StopGame;
    }

    void Update()
    {
        Time.timeScale = timeScale;
    }

    void StopGame()
    {
        timeScale = 0;
    }
}
