using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameSceneSO gameScene;
    public LoadEventChannelSO loadSceneEvent;

    public void LoadGameScene()
    {
        loadSceneEvent.RaiseEvent(gameScene, true);
    }
}
