using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

/// <summary>
/// This class is a used for scene loading events.
/// Takes an array of the scenes we want to load and a bool to specify if we want to show a loading screen.
/// </summary>
[CreateAssetMenu(menuName = "Events/Load Event Channel")]
public class LoadEventChannelSO : ScriptableObject
{
    public event UnityAction<GameSceneSO, bool> OnLoadingRequested;

    public void RaiseEvent(GameSceneSO sceneToLoad, bool showLoadingScreen)
    {
        if (OnLoadingRequested != null)
        {
            OnLoadingRequested.Invoke(sceneToLoad, showLoadingScreen);
        }
        else
        {
            Debug.LogWarning("A Scene loading was requested, but nobody picked it up." +
                             "Check why there is no SceneLoader already present, " +
                             "and make sure it's listening on this Load Event channel.");
        }
    }
}