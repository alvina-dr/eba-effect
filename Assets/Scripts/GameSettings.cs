using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]

public class GameSettings : ScriptableObject
{
    [Header("LIFE AND DAMAGE")]
    public int startLifeValue = 50;
    public int targetDamage = 10;


    [Header("SCORE")]
    public int maxPointPerTarget = 120;

    [Header("COMBOS | x = threshold, y = multiplier, z = health gain")]
    [SerializeField] public List<Vector3Int> comboTable;
}