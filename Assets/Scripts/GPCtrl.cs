using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GPCtrl : MonoBehaviour
{
    public enum LevelState
    {
        Running,
        Ending,
        Over
    }

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
    [SerializeField] LevelState levelState;
    [Header("DEBUG TOOLS")]
    [SerializeField] public bool computerMode;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
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
        levelState = LevelState.Running;
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
        if (levelState == LevelState.Ending && FindObjectOfType<TargetCtrl>() == null) //change later when target pool is done | bad
        {
            levelState = LevelState.Over;
            EndLevel();
        }
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
        if (levelState == LevelState.Running && _chrono >= CSV.targetDataArray[targetIncrement].spawnTime - CSV.targetDataArray[targetIncrement].duration)
        {
            //Debug.Log("target : " + CSV.targetDataArray[targetIncrement].spawnTime + " | " + CSV.targetDataArray[targetIncrement].duration + " | " + CSV.targetDataArray[targetIncrement].startPosition);
            CreateTarget(CSV.targetDataArray[targetIncrement]);
            targetIncrement++;
            if ((targetIncrement) == CSV.targetDataArray.Length)
            {
                levelState = LevelState.Ending;
            }
        }
    }

    public void DebugMenu()
    {

    }

    public void EndLevel()
    {
        UI.endMenu.gameObject.SetActive(true);
        UI.endMenu.transform.DOScale(1, 0.5f);
        UI.endMenu.UpdateTotalDestroyed(Player.numTargetDestroyed);
        UI.endMenu.UpdateEndScore(Player.currentScore);

    }
}
