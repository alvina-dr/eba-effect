using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPCtrl : MonoBehaviour
{
    public CSVReader CSV;
    public VibrationCtrl Vibration;
    public PlayerCtrl Player;
    public UICtrl UI;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] float targetFrequence;
    [SerializeField] int targetIncrement;
    [SerializeField] float _chrono;
    [SerializeField] bool levelEnded = false;


    public void Start()
    {
        CSV = GetComponent<CSVReader>();   
        Vibration = GetComponent<VibrationCtrl>();   
        Player = FindObjectOfType<PlayerCtrl>();   
        UI = FindObjectOfType<UICtrl>();   
    }

    //ici variable du fichier csv, on importe depuis gp ctrl
    void Update()
    {
        _chrono += Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    CreateTarget();
        //}
        //if (currentTime >= targetFrequence)
        //{
        //    CreateTarget();
        //    currentTime = 0;
        //}
        //currentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //DEBUG MENU
        }
        TargetLevelSetup();
    }


    public void CreateTarget(TargetData _targetData)
    {
        TargetCtrl _target = Instantiate(targetPrefab).GetComponent<TargetCtrl>();
        _target.targetData = _targetData;
        _target.transform.position = _targetData.startPosition;
        _target.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void TargetLevelSetup()
    {
        if (!levelEnded && _chrono >= CSV.targetDataList[targetIncrement].spawnTime)
        {
            Debug.Log("target : " + CSV.targetDataList[targetIncrement].spawnTime + " | " + CSV.targetDataList[targetIncrement].duration + " | " + CSV.targetDataList[targetIncrement].startPosition);
            CreateTarget(CSV.targetDataList[targetIncrement]);
            targetIncrement++;
            if ((targetIncrement) == CSV.targetDataList.Length) levelEnded = true;
        }
    }

    public void DebugMenu()
    {

    }
}
