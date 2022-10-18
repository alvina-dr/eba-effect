using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CSVReader : MonoBehaviour
{

    public TextAsset textAssetData;
    //choose only level 1 for now

    public TargetData[] targetDataArray;

    public void Awake()
    {
        if (DataHolder.instance != null)
        {
            textAssetData = DataHolder.instance.levelToLoad;
        }
        ReadCSV();
    }

    public void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);
        data = data.Take(data.Length - 1).ToArray();
        int tableSize = data.Length / 3;
        targetDataArray = new TargetData[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            targetDataArray[i] = ScriptableObject.CreateInstance<TargetData>();
            targetDataArray[i].spawnTime = float.Parse(data[3 * (i)])/1000; //first column
            targetDataArray[i].duration = float.Parse(data[3 * (i) + 1]); //second column
            string[] _startPosition = data[3 * (i) + 2].Split(new string[] { "f", "\n" }, System.StringSplitOptions.None);
            targetDataArray[i].startPosition = new Vector3(float.Parse(_startPosition[0]), float.Parse(_startPosition[1]), 0);  //third column
        }
    }
}
