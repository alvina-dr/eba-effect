using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCtrl : MonoBehaviour
{
    public class ComboEntry
    {

        public int comboNum;
        public int multiplier;

        public ComboEntry(int _comboNum, int _multiplier)
        {
            comboNum = _comboNum;
            multiplier = _multiplier;
        }
    }

    [SerializeField] public List<ComboEntry> comboTable = new List<ComboEntry>();
}
