using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class is a base class which contains what is common to all game scenes (Locations or Menus)
/// </summary>
[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data")]
public class GameSceneSO : ScriptableObject
{
    [Header("Information")] public AssetReference sceneReference;

    [Header("Sounds")] public AudioCueSO musicTrack;
}