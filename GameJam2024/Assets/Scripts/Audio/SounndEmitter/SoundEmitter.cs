using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;

    public delegate void SoundFinishedPlaying(SoundEmitter soundEmitter);

    public event SoundFinishedPlaying OnSoundFinishedPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="settings"></param>
    /// <param name="hasToLoop"></param>
    /// <param name="position"></param>
    public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
    {
        audioSource.clip = clip;
        settings.ApplyTo(audioSource);
        audioSource.transform.position = position;
        audioSource.loop = hasToLoop;
        audioSource.Play();

        if (!hasToLoop)
        {
            StartCoroutine(FinishedPlaying(clip.length));
        }
    }

    public void FadeMusicIn(AudioClip musicClip, AudioConfigurationSO settings, float duration, float startTime = 0f)
    {
        PlayAudioClip(musicClip, settings, true);
        audioSource.volume = 0f;

        //Start the clip at the same time the previous one left, if length allows
        //TODO: find a better way to sync fading songs
        if (startTime <= audioSource.clip.length)
        {
            audioSource.time = startTime;
        }

        audioSource.DOFade(1f, duration);
    }

    public float FadeMusicOut(float duration)
    {
        audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

        return audioSource.time;
    }

    private void OnFadeOutComplete()
    {
        NotifyBeingDone();
    }

    /// <summary>
    /// Used to check which music track is being played.
    /// </summary>
    public AudioClip GetClip()
    {
        return audioSource.clip;
    }

    /// <summary>
    /// Used when the game is unpaused, to pick up SFX from where they left.
    /// </summary>
    public void Resume()
    {
        audioSource.Play();
    }

    /// <summary>
    /// Used when the game is paused.
    /// </summary>
    public void Pause()
    {
        audioSource.Pause();
    }

    /// <summary>
    /// Used when the SFX finished playing. Called by the <c>AudioManager</c>.
    /// </summary>
    public void Stop()
    {
        audioSource.Stop();
    }

    public void Finish()
    {
        if (audioSource.loop)
        {
            audioSource.loop = false;
            float timeRemaining = audioSource.clip.length - audioSource.time;
            StartCoroutine(FinishedPlaying(timeRemaining));
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public bool IsLooping()
    {
        return audioSource.loop;
    }

    IEnumerator FinishedPlaying(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
    }

    private void NotifyBeingDone()
    {
        OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
    }
}