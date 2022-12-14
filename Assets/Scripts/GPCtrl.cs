using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

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
    public string musicName;
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
        Time.timeScale = 1;
        CSV = GetComponent<CSVReader>();   
        Player = FindObjectOfType<PlayerCtrl>();
        UI = FindObjectOfType<UICtrl>();
        Projectile = FindObjectOfType<ProjectilePool>();
        Combo = GetComponent<ComboCtrl>();
        levelState = LevelState.Before;
        AudioEngine.instance.PlayMusic(null, false);
        _chrono -= offset;
        if (UI.fadeMaterial.color.a > 0)
        {
            UI.fadeMaterial.DOFade(1, 1f).OnComplete(() =>
            {
                UI.fadeMaterial.DOFade(0, .3f);
            });
        }

    }

    void Update()
    {
        if (Input.GetButton("XRI_Left_PrimaryButton")) UI.OpenPauseMenu();
        if (Input.GetButton("XRI_Right_PrimaryButton")) UI.OpenPauseMenu();
        if (computerMode && Input.GetKeyDown(KeyCode.Escape)) UI.OpenPauseMenu();
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
        if (levelState == LevelState.Ending && _chrono >= musicDuration + DataHolder.instance.GameSettings.endMusicOffset) WinGame();
    }


    public void CreateTarget(TargetData _targetData)
    {
        TargetCtrl _target = Instantiate(targetPrefab, targetPool.transform).GetComponent<TargetCtrl>();
        _target.targetData = _targetData;
        _target.transform.position = _targetData.startPosition;
        //rightTargetIndicator.MoveToFirstTarget();
        //leftTargetIndicator.MoveToFirstTarget();
    }

    public void TargetLevelSetup()
    {
        //if (levelState == LevelState.Running && automaticTimingMode) //here we are in automatic mode that makes target spawn if energy is higher than threshold
        //{
        //    CreateTarget(CSV.targetDataArray[targetIncrement]);
        //    if (AudioEngine.instance.)
        //    return;
        //}

        if (levelState == LevelState.Running && _chrono >= CSV.targetDataArray[targetIncrement].spawnTime - CSV.targetDataArray[targetIncrement].duration/2)
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
        levelState = LevelState.Over;
        AudioEngine.instance.PlaySound(DataHolder.instance.GameSettings.winSound, false);
        UI.endMenu.UpdateTitle(true);
        EndLevel();
    }

    public void LooseGame()
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
