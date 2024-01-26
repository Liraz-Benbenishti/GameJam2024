using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>
public class InitializationLoader : MonoBehaviour
{
    [Header("Persistent managers Scene")]
    [SerializeField] private GameSceneSO persistentManagersScene;

    [Header("Loading settings")]
    [SerializeField] private bool showLoadScreen;

    private void Start()
    {
        Debug.Log("Starting to load persistent managers scene");
        persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, showLoadScreen).Completed += LoadEventChannel;
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        Debug.Log("Successfully loaded persistent managers scene removing init scene");
        SceneManager.UnloadSceneAsync(0);
    }
}