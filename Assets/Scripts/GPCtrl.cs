using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GPCtrl : MonoBehaviour
{
    public enum LevelState
    {
        Before,
        Running,
        Ending,
        Over
    }

    public static GPCtrl instance = null;
    public CSVReader CSV;
    public PlayerCtrl Player;
    public UICtrl UI;
    public ProjectilePool Projectile;
    public ComboCtrl Combo;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] float targetFrequence;
    [SerializeField] int targetIncrement;
    [SerializeField] float _chrono;
    [SerializeField] public LevelState levelState;
    public AudioClip levelMusic;
    public float offset;
    public float bpm;
    public float musicDuration;
    public TargetIndicator rightTargetIndicator;
    public TargetIndicator leftTargetIndicator;
    [SerializeField] public GameObject targetPool;

    [Header("DEBUG TOOLS")]
    [SerializeField] public bool computerMode;
    [SerializeField] public bool cheatMode;
    [SerializeField] public bool lowPassFilter;
    [SerializeField] public bool automaticTimingMode;

    [Header("Automatic timing mode")]
    [SerializeField] float energy;
    [SerializeField] float threshold;
    public AnimationCurve inLowPass;
    public AnimationCurve outLowPass;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        CSV = GetComponent<CSVReader>();   
        Player = FindObjectOfType<PlayerCtrl>();
        UI = FindObjectOfType<UICtrl>();
        Projectile = FindObjectOfType<ProjectilePool>();
        Combo = GetComponent<ComboCtrl>();
        levelState = LevelState.Before;
        AudioEngine.instance.PlayMusic(null, false);
        _chrono -= offset;
    }

    //ici variable du fichier csv, on importe depuis gp ctrl
    void Update()
    {
        if (levelState == LevelState.Before && FindObjectOfType<TargetCtrl>() == null)
        {
            LaunchLevel();
            levelState = LevelState.Running;
            return;
        } else if (levelState == LevelState.Before)
        {
            return;
        }
        _chrono += Time.deltaTime;
        TargetLevelSetup();
        if (levelState == LevelState.Ending && FindObjectOfType<TargetCtrl>() == null) //change later when target pool is done | bad
        {
            levelState = LevelState.Over;
            WinGame();
        }
    }


    public void CreateTarget(TargetData _targetData)
    {
        TargetCtrl _target = Instantiate(targetPrefab, targetPool.transform).GetComponent<TargetCtrl>();
        _target.targetData = _targetData;
        _target.transform.position = _targetData.startPosition;
        rightTargetIndicator.MoveToFirstTarget();
        leftTargetIndicator.MoveToFirstTarget();
    }

    public void TargetLevelSetup()
    {
        //if (levelState == LevelState.Running && automaticTimingMode) //here we are in automatic mode that makes target spawn if energy is higher than threshold
        //{
        //    CreateTarget(CSV.targetDataArray[targetIncrement]);
        //    if (AudioEngine.instance.)
        //    return;
        //}

        if (levelState == LevelState.Running && _chrono >= CSV.targetDataArray[targetIncrement].spawnTime - CSV.targetDataArray[targetIncrement].duration + DataHolder.instance.GameSettings.targetOffset )
        {
            CreateTarget(CSV.targetDataArray[targetIncrement]);
            targetIncrement++;
            if ((targetIncrement) == CSV.targetDataArray.Length)
            {
                levelState = LevelState.Ending;
            }
        }
    }

    public void WinGame()
    {
        AudioEngine.instance.PlaySound(DataHolder.instance.GameSettings.winSound, false);
        UI.endMenu.UpdateTitle(true);
        EndLevel();
    }

    public void GameOver()
    {
        if (cheatMode) return;
        levelState = LevelState.Over; //will need to destroy all targets when game over
        //for (int i = 0; i < FindObjectsOfType<TargetCtrl>().Length; i++)
        //{
        //    Destroy(FindObjectsOfType<TargetCtrl>()[0].gameObject);
        //    i--;
        //}
        UI.endMenu.UpdateTitle(false);
        AudioEngine.instance.musicStream.DOPitch(0, .5f).OnComplete(() => {
            AudioEngine.instance.musicStream.volume = 0;

            AudioEngine.instance.musicStream.Stop();
            //DOTween.To(() => AudioEngine.instance.musicStream.volume, x => AudioEngine.instance.musicStream.volume = x, 0, .2f);

            AudioEngine.instance.musicStream.DOPitch(1, .1f).OnComplete(() => {
                AudioEngine.instance.musicStream.volume = 1;
                AudioEngine.instance.lowPass.cutoffFrequency = 22000;
            });
        });
        EndLevel();
    }

    public void EndLevel()
    {
        UI.endMenu.gameObject.SetActive(true);
        UI.inGameMenu.gameObject.SetActive(false);
        UI.endMenu.transform.DOScale(1, 0.5f);
        UI.endMenu.UpdateTotalDestroyed(Player.numTargetDestroyed);
        UI.endMenu.UpdateEndScore(Player.currentScore);
        UI.endMenu.UpdateMaxCombo(Player.maxCombo);
    }

    public void LaunchLevel()
    {
        if (levelMusic != null) AudioEngine.instance.PlayMusic(levelMusic, false);
    }
}
