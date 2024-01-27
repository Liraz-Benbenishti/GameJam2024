using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public AudioCueEventChannelSO playSfxEvent;
    public AudioConfigurationSO sfxConfig;
    public AudioCueSO collideSfx;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collideSfx != null && other.gameObject.CompareTag("Player"))
        {
            playSfxEvent.RaisePlayEvent(collideSfx, sfxConfig);
        }
    }
}
