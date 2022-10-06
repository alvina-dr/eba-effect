using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{

    public TextAsset textAssetData;

    [System.Serializable]
    public class TargetData
    {
        public float spawnTime;
    }

    public TargetData[] targetDataList;

    public void Start()
    {
        ReadCSV();
    }

    public void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ", ", "\n" }, System.StringSplitOptions.None);
        int tableSize = data.Length / 1;
        for (int i = 0; i < tableSize; i++)
        {
            targetDataList[i].spawnTime = float.Parse(data[i]);
        }
    }
}
