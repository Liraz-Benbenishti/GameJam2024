using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have a float argument.
/// </summary>
[CreateAssetMenu(menuName = "Events/Float Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
    [SerializeField] bool logEvents;

    public event UnityAction<float> OnEventRaised;

    public void RaiseEvent(float value)
    {
        OnEventRaised?.Invoke(value);
    }
}