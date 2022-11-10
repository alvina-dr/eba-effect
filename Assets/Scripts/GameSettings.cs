using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]

public class GameSettings : ScriptableObject
{
    [Header("LIFE AND DAMAGE")]
    public int startLifeValue;
    public int targetDamage;

    [Header("COMBOS | x = threshold, y = multiplier, z = health gain")]
    [SerializeField] public List<Vector3Int> comboTable;
}
