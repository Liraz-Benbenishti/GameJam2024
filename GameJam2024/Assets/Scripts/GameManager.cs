using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameTimeInSeconds = 10f; // Adjust this to set the total game time in seconds.
    private float currentTime;
    public bool isGameOver = false;
    public TMP_Text countdownText; // Reference to a UI Text component to display the countdown.
    public VoidEventChannel gameOverEvent;
    void Start()
    {
        currentTime = gameTimeInSeconds;
        UpdateCountdownText();
    } 

    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime; // Countdown time.
            
            // Update the UI Text to display the countdown.
            UpdateCountdownText();
        }
        else
        {
            // Game over or handle the end of the game logic here.
            Debug.Log("Game Over!");
            gameOverEvent.raiseEvent();
        }
    }

    void UpdateCountdownText()
    {
        // Update the UI Text to display the current time.
        if (countdownText != null)
        {
            
            countdownText.text = "Time: " + Mathf.CeilToInt(currentTime);
        }
    }
}