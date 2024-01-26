using UnityEngine;

public class StartUp : MonoBehaviour
{
    [SerializeField] GameSceneSO lobbyScene;
    [SerializeField] LoadEventChannelSO loadSceneEvent;
    
    void Start()
    {
        loadSceneEvent.RaiseEvent(lobbyScene, true);
    }
}
