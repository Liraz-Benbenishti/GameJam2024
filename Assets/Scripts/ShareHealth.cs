using System;
using UnityEngine;

/// <summary>
/// This class is used for sharing float variables.
/// Example: Health value that changes in character script and read in UI script
/// </summary>
[CreateAssetMenu(menuName = "Variables/Float")]
public class IntHeartVariable : ScriptableObject
{
    [SerializeField]public  int initialHealth;
    [SerializeField] public GameUI gameUI;

    public int health { get; set; }

    void Awake()
    {
        health = initialHealth;
    }
}