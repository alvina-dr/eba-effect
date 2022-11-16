using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCtrl : MonoBehaviour
{   
    public int ApplyMultiplierToScore(int _score, int _combo)
    {
        for (int i = 0; i < DataHolder.instance.GameSettings.comboTable.Count; i++)
        {
            if (_combo < DataHolder.instance.GameSettings.comboTable[i].x) return _score * DataHolder.instance.GameSettings.comboTable[i].y;
        }
        return _score * DataHolder.instance.GameSettings.comboTable[DataHolder.instance.GameSettings.comboTable.Count - 1].y;
    }
}
