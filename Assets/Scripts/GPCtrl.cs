using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPCtrl : MonoBehaviour
{
    public static GPCtrl instance = null;
    public CSVReader CSV;
    public VibrationCtrl Vibration;
    public PlayerCtrl Player;
    public UICtrl UI;
    public ProjectilePool Projectile;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] float targetFrequence;
    [SerializeField] int targetIncrement;
    [SerializeField] float _chrono;
    [SerializeField] bool levelEnded = false;
    [Header("DEBUG TOOLS")]
    [SerializeField] public bool computerMode;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        CSV = GetComponent<CSVReader>();   
        Vibration = GetComponent<VibrationCtrl>();   
        Player = FindObjectOfType<PlayerCtrl>();   
        UI = FindObjectOfType<UICtrl>();
        Projectile = FindObjectOfType<ProjectilePool>();   
    }

    //ici variable du fichier csv, on importe depuis gp ctrl
    void Update()
    {
        _chrono += Time.deltaTime;
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
        if (!levelEnded && _chrono >= CSV.targetDataArray[targetIncrement].spawnTime - CSV.targetDataArray[targetIncrement].duration)
        {
            Debug.Log("target : " + CSV.targetDataArray[targetIncrement].spawnTime + " | " + CSV.targetDataArray[targetIncrement].duration + " | " + CSV.targetDataArray[targetIncrement].startPosition);
            CreateTarget(CSV.targetDataArray[targetIncrement]);
            targetIncrement++;
            if ((targetIncrement) == CSV.targetDataArray.Length) levelEnded = true;
        }
    }

    public void DebugMenu()
    {

    }
}
