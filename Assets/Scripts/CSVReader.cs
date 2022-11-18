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
        float _duration = 0;
        List<string> data = textAssetData.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None).ToList();
        data.RemoveAt(data.Count - 1);
        
        GPCtrl.instance.levelMusic = Resources.Load<AudioClip>("Musics/"+data[0]);
        data.RemoveAt(0);
        
        if (data[0] != null) GPCtrl.instance.offset = float.Parse(data[0])/1000;
        data.RemoveAt(0);

        if(data[0] != null) GPCtrl.instance.bpm = float.Parse(data[0]);
        data.RemoveAt(0);

        if (data[0] != null) _duration = float.Parse(data[0]);
        data.RemoveAt(0);
        
        int tableSize = data.Count / 4;
        targetDataArray = new TargetData[tableSize];
        for (int i = 0; i < tableSize; i++)
        {
            targetDataArray[i] = ScriptableObject.CreateInstance<TargetData>();
            targetDataArray[i].spawnTime = float.Parse(data[4 * (i)])/1000; //first column
            targetDataArray[i].duration = _duration;/*float.Parse(data[4 * (i) + 1]);*/ //second column
            string[] _startPosition = data[4 * (i) + 1].Split(new string[] { "f", "\n" }, System.StringSplitOptions.None);
            targetDataArray[i].startPosition = new Vector3(float.Parse(_startPosition[0])*2, float.Parse(_startPosition[1])*1.5f+1, 0);  //third column
            targetDataArray[i].targetSide = (TargetData.TargetSide)System.Enum.Parse(typeof(TargetData.TargetSide), data[4 * (i) + 2]); //first column
        }
    }
}
