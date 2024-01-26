using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("SoundEmitters pool")] [SerializeField]
    private SoundEmitterPoolSO pool;

    [SerializeField] private int initialSize = 10;

    [Header("Listening on channels")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
    [SerializeField]
    private AudioCueEventChannelSO SFXEventChannel;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")] [SerializeField]
    private AudioCueEventChannelSO musicEventChannel;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
    [SerializeField]
    private FloatEventChannelSO sfxVolumeEventChannel;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
    [SerializeField]
    private FloatEventChannelSO musicVolumeEventChannel;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
    [SerializeField]
    private FloatEventChannelSO masterVolumeEventChannel;

    [Header("Audio control")] [SerializeField]
    private AudioMixer audioMixer;

    [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;

    private SoundEmitterVault soundEmitterVault;
    private SoundEmitter musicSoundEmitter;

    private void Awake()
    {
        //TODO: Get the initial volume levels from the settings
        soundEmitterVault = new SoundEmitterVault();

        pool.Prewarm(initialSize);
        pool.SetParent(transform);

        SetGroupVolume("MasterVolume", masterVolume);
        SetGroupVolume("MusicVolume", musicVolume);
        SetGroupVolume("SfxVolume", sfxVolume);
    }

    private void OnEnable()
    {
        SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
        SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
        SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

        musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
        musicEventChannel.OnAudioCueStopRequested += StopMusic;

        masterVolumeEventChannel.OnEventRaised += ChangeMasterVolume;
        musicVolumeEventChannel.OnEventRaised += ChangeMusicVolume;
        sfxVolumeEventChannel.OnEventRaised += ChangeSFXVolume;
    }

    private void OnDestroy()
    {
        SFXEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
        SFXEventChannel.OnAudioCueStopRequested -= StopAudioCue;
        SFXEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;

        musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;
        musicEventChannel.OnAudioCueStopRequested -= StopMusic;

        masterVolumeEventChannel.OnEventRaised -= ChangeMasterVolume;
        musicVolumeEventChannel.OnEventRaised -= ChangeMusicVolume;
        sfxVolumeEventChannel.OnEventRaised -= ChangeSFXVolume;
    }

    /// <summary>
    /// This is only used in the Editor, to debug volumes.
    /// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetGroupVolume("MasterVolume", masterVolume);
            SetGroupVolume("MusicVolume", musicVolume);
            SetGroupVolume("SfxVolume", sfxVolume);
        }
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
        if (!volumeSet)
        {
            Debug.LogError("The AudioMixer parameter was not found");
        }
    }

    public float GetGroupVolume(string parameterName)
    {
        if (audioMixer.GetFloat(parameterName, out float rawVolume))
        {
            return MixerValueToNormalized(rawVolume);
        }

        Debug.LogError("The AudioMixer parameter was not found");
        return 0f;
    }

    private void ChangeMasterVolume(float newVolume)
    {
        Debug.Log($"Changing volume to {newVolume}");
        SetGroupVolume("MasterVolume", newVolume);
    }

    private void ChangeMusicVolume(float newVolume)
    {
        Debug.Log($"Changing music volume to {newVolume}");
        SetGroupVolume("MusicVolume", newVolume);
    }

    private void ChangeSFXVolume(float newVolume)
    {
        Debug.Log($"Changing sfx volume to {newVolume}");
        SetGroupVolume("SfxVolume", newVolume);
    }

    // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
    /// when using UI sliders normalized format
    private float MixerValueToNormalized(float mixerValue)
    {
        // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
        return 1f + (mixerValue / 80f);
    }

    private float NormalizedToMixerValue(float normalizedValue)
    {
        // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
        // This doesn't allow values over 0dB
        return (normalizedValue - 1f) * 80f;
    }

    private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration,
        Vector3 positionInSpace)
    {
        float fadeDuration = 2f;
        float startTime = 0f;

        if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
        {
            AudioClip songToPlay = audioCue.GetClips()[0];
            if (musicSoundEmitter.GetClip() == songToPlay)
            {
                return AudioCueKey.Invalid;
            }

            //Music is already playing, need to fade it out
            startTime = musicSoundEmitter.FadeMusicOut(fadeDuration);
        }

        musicSoundEmitter = pool.Request();
        musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);
        musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

        return AudioCueKey.Invalid; //No need to return a valid key for music
    }

    private bool StopMusic(AudioCueKey key)
    {
        if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
        {
            musicSoundEmitter.Stop();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
    /// </summary>
    public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
    {
        AudioClip[] clipsToPlay = audioCue.GetClips();
        SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

        int nOfClips = clipsToPlay.Length;
        for (int i = 0; i < nOfClips; i++)
        {
            soundEmitterArray[i] = pool.Request();
            if (soundEmitterArray[i] != null)
            {
                soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);
                if (!audioCue.looping)
                {
                    soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }
        }

        return soundEmitterVault.Add(audioCue, soundEmitterArray);
    }

    public bool FinishAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

        if (isFound)
        {
            for (int i = 0; i < soundEmitters.Length; i++)
            {
                soundEmitters[i].Finish();
                soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }
        else
        {
            Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
        }

        return isFound;
    }

    public bool StopAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

        if (isFound)
        {
            for (int i = 0; i < soundEmitters.Length; i++)
            {
                StopAndCleanEmitter(soundEmitters[i]);
            }

            soundEmitterVault.Remove(audioCueKey);
        }

        return isFound;
    }

    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        StopAndCleanEmitter(soundEmitter);
    }

    private void StopAndCleanEmitter(SoundEmitter soundEmitter)
    {
        if (!soundEmitter.IsLooping())
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

        soundEmitter.Stop();
        pool.Return(soundEmitter);

        //TODO: is the above enough?
        //_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
        //How is the key removed from the vault?
    }

    private void StopMusicEmitter(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
        pool.Return(soundEmitter);
    }
}