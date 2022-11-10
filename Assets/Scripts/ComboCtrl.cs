using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCtrl : MonoBehaviour
{
    [Header("x = threshold, y = multiplier, z = health gain")]
    [SerializeField] public List<Vector3Int> comboTable;


    

    public int ApplyMultiplierToScore(int _score, int _combo)
    {
        for (int i = 0; i < comboTable.Count; i++)
        {
            if (_combo < comboTable[i].x) return _score * comboTable[i].y;
        }
        return _score * comboTable[comboTable.Count - 1].y;
    }
}
