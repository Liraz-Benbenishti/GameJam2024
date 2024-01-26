using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Event on which <c>AudioCue</c> components send a message to play SFX and music. <c>AudioManager</c> listens on these events, and actually plays the sound.
/// </summary>
[CreateAssetMenu(menuName = "Events/AudioCue Event Channel")]
public class AudioCueEventChannelSO : ScriptableObject
{
    [SerializeField] bool logEvents;

    public event AudioCuePlayAction OnAudioCuePlayRequested;
    public event AudioCueStopAction OnAudioCueStopRequested;
    public event AudioCueFinishAction OnAudioCueFinishRequested;

    public AudioCueKey RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration,
        Vector3 positionInSpace = default)
    {
        AudioCueKey audioCueKey = AudioCueKey.Invalid;

        if (OnAudioCuePlayRequested != null)
        {
#if UNITY_EDITOR
            if (logEvents)
            {
                var method = new StackFrame(1).GetMethod();
                var callingClassName = method.DeclaringType?.Name;
                var callingFuncName = method.Name;
                Debug.Log($"Event {name} has been raised by {callingClassName}.{callingFuncName}");
            }
#endif
            audioCueKey = OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration, positionInSpace);
        }
        else
        {
            Debug.LogWarning("An AudioCue play event was requested, but nobody picked it up. " +
                             "Check why there is no AudioManager already loaded, " +
                             "and make sure it's listening on this AudioCue Event channel.");
        }

        return audioCueKey;
    }

    public bool RaiseStopEvent(AudioCueKey audioCueKey)
    {
        bool requestSucceed = false;

        if (OnAudioCueStopRequested != null)
        {
#if UNITY_EDITOR
            if (logEvents)
            {
                var method = new StackFrame(1).GetMethod();
                var callingClassName = method.DeclaringType?.Name;
                var callingFuncName = method.Name;
                Debug.Log($"Event {name} has been raised by {callingClassName}.{callingFuncName}");
            }
#endif
            requestSucceed = OnAudioCueStopRequested.Invoke(audioCueKey);
        }
        else
        {
            Debug.LogWarning("An AudioCue stop event was requested, but nobody picked it up. " +
                             "Check why there is no AudioManager already loaded, " +
                             "and make sure it's listening on this AudioCue Event channel.");
        }

        return requestSucceed;
    }

    public bool RaiseFinishEvent(AudioCueKey audioCueKey)
    {
        bool requestSucceed = false;

        if (OnAudioCueStopRequested != null)
        {
#if UNITY_EDITOR
            if (logEvents)
            {
                var method = new StackFrame(1).GetMethod();
                var callingClassName = method.DeclaringType?.Name;
                var callingFuncName = method.Name;
                Debug.Log($"Event {name} has been raised by {callingClassName}.{callingFuncName}");
            }
#endif
            requestSucceed = OnAudioCueFinishRequested.Invoke(audioCueKey);
        }
        else
        {
            Debug.LogWarning("An AudioCue finish event was requested, but nobody picked it up. " +
                             "Check why there is no AudioManager already loaded, " +
                             "and make sure it's listening on this AudioCue Event channel.");
        }

        return requestSucceed;
    }
}

public delegate AudioCueKey AudioCuePlayAction(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration,
    Vector3 positionInSpace);

public delegate bool AudioCueStopAction(AudioCueKey emitterKey);

public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);