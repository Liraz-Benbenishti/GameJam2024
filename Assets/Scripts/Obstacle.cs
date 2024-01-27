using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public AudioCueEventChannelSO playSfxEvent;
    public AudioConfigurationSO sfxConfig;
    public AudioCueSO collideSfx;

    public bool shouldDestoryOnCillision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (collideSfx != null)
            {
                playSfxEvent.RaisePlayEvent(collideSfx, sfxConfig);
            }
            if (shouldDestoryOnCillision)
            {
                Destroy(gameObject);
            }
        }
    }
}
