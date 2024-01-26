using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("Load Events")] [SerializeField]
    private LoadEventChannelSO loadScene;

    [SerializeField] VoidEventChannel onExitGame;

    [Header("Broadcasting on")] [SerializeField]
    private BoolEventChannelSO toggleLoadingScreen;
    
    [Header("Audio")] [SerializeField] AudioCueEventChannelSO playMusicOn;
    [SerializeField] AudioConfigurationSO audioConfig;

    private AsyncOperationHandle<SceneInstance> loadingOperationHandle;

    private GameSceneSO sceneToLoad;
    private GameSceneSO currentlyLoadedScene;
    AudioCueKey? currentSceneMusic;
    private bool showLoadingScreen;

    private bool isLoading;

    private void OnEnable()
    {
        loadScene.OnLoadingRequested += LoadScene;
        onExitGame.event_raised += ExitGame;
    }

    private void OnDisable()
    {
        loadScene.OnLoadingRequested -= LoadScene;
        onExitGame.event_raised -= ExitGame;
    }

    /// <summary>
    /// This function loads the location scenes passed as array parameter
    /// </summary>
    /// <param name="sceneToLoad"></param>
    /// <param name="showLoadingScreen"></param>
    private void LoadScene(GameSceneSO sceneToLoad, bool showLoadingScreen)
    {
        if (!isLoading)
        {
            this.sceneToLoad = sceneToLoad;
            this.showLoadingScreen = showLoadingScreen;
            isLoading = true;

            StartCoroutine(UnloadPreviousScene());
        }
    }

    /// <summary>
    /// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
    /// </summary>
    private IEnumerator UnloadPreviousScene()
    {
        // _inputReader.DisableAllInput();
        // _fadeRequestChannel.FadeOut(_fadeDuration);

        yield return new WaitForSeconds(0);

        // if (currentSceneMusic != null)
        // {
        //     playMusicOn.RaiseStopEvent(currentSceneMusic.Value);
        //     currentSceneMusic = null;
        // }

        if (currentlyLoadedScene != null)
        {
            if (currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
            {
                currentlyLoadedScene.sceneReference.UnLoadScene();
            }
#if UNITY_EDITOR
            else
            {
                //Only used when, after a "cold start", the player moves to a new scene
                //Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                //the scene needs to be unloaded using regular SceneManager instead of as an Addressable
                SceneManager.UnloadSceneAsync(currentlyLoadedScene.sceneReference.editorAsset.name);
            }
#endif
        }

        LoadNewScene();
    }

    /// <summary>
    /// Kicks off the asynchronous loading of a scene, either menu or Location.
    /// </summary>
    private void LoadNewScene()
    {
        if (showLoadingScreen)
        {
            toggleLoadingScreen.RaiseEvent(true);
        }

        loadingOperationHandle = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        loadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        currentlyLoadedScene = sceneToLoad;

        var s = obj.Result.Scene;
        SceneManager.SetActiveScene(s);
        LightProbes.TetrahedralizeAsync();

        isLoading = false;

        if (showLoadingScreen) toggleLoadingScreen.RaiseEvent(false);

        // _fadeRequestChannel.FadeIn(_fadeDuration);

        // StartGameplay();
        if (currentlyLoadedScene.musicTrack != null)
        {
            currentSceneMusic = playMusicOn.RaisePlayEvent(currentlyLoadedScene.musicTrack, audioConfig);
        }

        // onSceneReady.raiseEvent();
        // onSceneLoaded.RaiseEvent(s);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
    }
}