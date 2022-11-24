using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]

public class GameSettings : ScriptableObject
{
    [Header("LIFE AND DAMAGE")]
    public int startLifeValue = 50;
    public int targetDamage = 10;

    [Header("HAPTIC")]
    public float vibrationDuration = 0.1f;
    public float vibrationOffset = 0.5f;

    [Header("SCORE")]
    public int maxPointPerTarget = 120;

    [Header("COMBOS | x = threshold, y = multiplier, z = health gain")]
    public List<Vector3Int> comboTable;
    public int goodSideMultiplier;

    [Header("SOUND FX")]
    public AudioClip winSound;
    public AudioClip gunSound;
    public AudioClip windSound;

    [Header("MUSIC")]
    public AudioClip mainMenuMusic;
    public float endMusicOffset = 0;

    [Header("OFFSET TARGET LIFE")]
    public float targetOffset;
    public float pauseOnHit;

}
