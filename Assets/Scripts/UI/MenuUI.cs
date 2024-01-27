using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameSceneSO gameScene;
    public LoadEventChannelSO loadSceneEvent;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Debug.Log("Starting menu animation");
        animator.SetTrigger("Start");
    }

    public void LoadGameScene()
    {
        animator.StopPlayback();
        loadSceneEvent.RaiseEvent(gameScene, true);
    }
}
