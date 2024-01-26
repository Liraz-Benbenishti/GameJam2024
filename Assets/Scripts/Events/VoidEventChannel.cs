
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "event/voidEvent")]
public class VoidEventChannel : ScriptableObject
{
    public event UnityAction event_raised;

    public void raiseEvent()
    {
       event_raised?.Invoke();
    }
}
