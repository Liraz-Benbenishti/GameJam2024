using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public VoidEventChannel gameOverEvent;
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void showGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    private void OnEnable()
    {
        gameOverEvent.event_raised += showGameOverPanel;
    }

    private void OnDisable()
    {
        gameOverEvent.event_raised -= showGameOverPanel;
    }
}
