using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TargetCtrl : MonoBehaviour
{
    public TargetData targetData;
    float chrono;
    private GPCtrl GP;
    [SerializeField] private GameObject timingIndicator;
    bool animationCanLoop = true;
    [SerializeField] GameObject targetHinge;
    [SerializeField] MeshRenderer targetHingeRenderer;
    [SerializeField] SkinnedMeshRenderer helixRenderer;
    [SerializeField] Material redTargetMaterial;
    [SerializeField] Material redTargetEmissionMaterial;
    [SerializeField] Material blueTargetMaterial;
    [SerializeField] Material blueTargetEmissionMaterial;

    [SerializeField] Material redIndicatorMaterial;
    [SerializeField] Material blueIndicatorMaterial;
    [SerializeField] ParticleSystem blueParticle;
    [SerializeField] ParticleSystem redParticle;
    [SerializeField] TextMeshPro scoreText;


    private void Start()
    {
        GP = GPCtrl.instance;
        blueParticle.gameObject.SetActive(false);
        redParticle.gameObject.SetActive(false);
        transform.rotation *= Quaternion.Euler(0, -90, 0);
        if (GP == null) return;
        transform.LookAt(Camera.main.transform);
        transform.rotation *= Quaternion.Euler(0, -90, 0);

        if (GP.levelState == GPCtrl.LevelState.Before) return;
        if (targetData.targetSide == TargetData.TargetSide.right)
        {
            helixRenderer.material = blueTargetMaterial; 
            targetHingeRenderer.material = blueTargetMaterial;
            timingIndicator.GetComponent<MeshRenderer>().material = blueIndicatorMaterial;
        }
        timingIndicator.transform.localScale = new Vector3(1000, 1000, 1000);
        Debug.Log(targetData.duration - DataHolder.instance.GameSettings.targetOffset);
        timingIndicator.transform.DOScale(150f, targetData.duration/2).SetEase(Ease.Linear).OnComplete(() =>
        {
            timingIndicator.transform.localScale = new Vector3(1, 1, 1);
            if (targetData.targetSide == TargetData.TargetSide.right)
            {
                helixRenderer.material = blueTargetEmissionMaterial;
                targetHingeRenderer.material = blueTargetEmissionMaterial;
            } else
            {
                helixRenderer.material = redTargetEmissionMaterial;
                targetHingeRenderer.material = redTargetEmissionMaterial;
            }
            timingIndicator.transform.DOScale(0.0085f, targetData.duration / 2).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GetComponent<BoxCollider>().enabled) return;
                transform.SetParent(null);
                //GP.rightTargetIndicator.MoveToFirstTarget();
                //GP.leftTargetIndicator.MoveToFirstTarget();
                transform.DOScale(0, 0.2f).OnComplete(() => {
                    GP.Player.currentCombo = 0;
                    GP.Player.scoreMultiplier = 1;
                    GP.Player.health -= DataHolder.instance.GameSettings.targetDamage;
                    GP.UI.UpdateLifeBar(GP.Player.health);
                    GP.UI.UpdateCombo(GP.Player.currentCombo);
                    Destroy(gameObject);
                    if (GP.lowPassFilter) return;
                    DOTween.To(() => AudioEngine.instance.lowPass.cutoffFrequency, x => AudioEngine.instance.lowPass.cutoffFrequency = x, 0, GP.bpm * 0.00171875f ).SetEase(GP.inLowPass).OnComplete(() => { //shoud last 0,00171875 * bpm .22f
                        DOTween.To(() => AudioEngine.instance.lowPass.cutoffFrequency, x => AudioEngine.instance.lowPass.cutoffFrequency = x, 22000, GP.bpm * 0.00171875f).SetEase(GP.outLowPass);
                    });
                });
            });
        });
    }

    void Update()
    {
        if (animationCanLoop)
        {
            animationCanLoop = false;
            transform.DOMoveY(transform.position.y + 0.05f, 1f).OnComplete(() =>
            {
                transform.DOMoveY(transform.position.y - 0.05f, 1f).OnComplete(() => {
                    animationCanLoop = true;
                });
            });
        }
        chrono += Time.deltaTime;

    }

    public void DestroyTargetOnHit(TargetData.TargetSide targetSide)
    {
        GP.Player.currentCombo++;
        GP.UI.UpdateCombo(GP.Player.currentCombo);
        int percentage =  Mathf.RoundToInt(chrono / (targetData.duration) * 100);
        int _scoreBeforeSideMultiplier = 0;
        if (percentage > 100)
        {
            Debug.Log("too late : " + (100 - (percentage-100)));

        } else
        {
            Debug.Log("too early : " + percentage);
        }
        if (percentage > 100)
        {
            _scoreBeforeSideMultiplier = Mathf.RoundToInt((100 - (percentage - 100)) * (DataHolder.instance.GameSettings.maxPointPerTarget - DataHolder.instance.GameSettings.minPointPerTargetEnd) / 100) + DataHolder.instance.GameSettings.minPointPerTargetEnd;
        } else
        {
            _scoreBeforeSideMultiplier = Mathf.RoundToInt(percentage * (DataHolder.instance.GameSettings.maxPointPerTarget - DataHolder.instance.GameSettings.minPointPerTargetStart) / 100) + DataHolder.instance.GameSettings.minPointPerTargetStart;
        }
        //int _score = Mathf.RoundToInt(DataHolder.instance.GameSettings.maxPointPerTarget * percentage / 100);
        int _score = _scoreBeforeSideMultiplier;
        if (targetSide == targetData.targetSide) Debug.Log("good side"); _score *= DataHolder.instance.GameSettings.goodSideMultiplier;
        Debug.Log("this is the score : " + _score);
        TextMeshPro _scoreText = Instantiate(scoreText);
        _scoreText.text = _score.ToString();
        _scoreText.transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z - .5f);
        _scoreText.transform.localScale = new Vector3(.2f, .2f, .2f);
        _scoreText.transform.DOScale(.3f, .5f);
        _scoreText.transform.DOLocalMove(_scoreText.transform.localPosition + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), -2f ), .5f);
        _scoreText.DOFade(0, .7f).OnComplete(() =>
        {
            Destroy(_scoreText.gameObject);
        });
        _score = GP.Combo.ApplyMultiplierToScore(_score, GP.Player.currentCombo);
        GP.Player.currentScore += _score;
        GP.UI.UpdateScore(GP.Player.currentScore);
        GP.Player.health += 5; //hard value need to be variable to tweak later
        if (GP.Player.health > 100) GP.Player.health = 100;
        GP.UI.UpdateLifeBar(GP.Player.health);
        GP.Player.numTargetDestroyed++;
        if (GP.Player.currentCombo > GP.Player.maxCombo) GP.Player.maxCombo = GP.Player.currentCombo;
        GetComponent<BoxCollider>().enabled = false;
        transform.SetParent(null);
        //GP.rightTargetIndicator.MoveToFirstTarget();
        //GP.leftTargetIndicator.MoveToFirstTarget();        
        if (targetData.targetSide == TargetData.TargetSide.left) redParticle.gameObject.SetActive(true);
        else blueParticle.gameObject.SetActive(true);
        StartCoroutine(PauseGame(DataHolder.instance.GameSettings.pauseOnHit));
        targetHinge.transform.DORotate(targetHinge.transform.eulerAngles + new Vector3(-90, 0, 0), .3f).OnComplete(() => {
            transform.DOScale(0.35f, 0.1f).OnComplete(() => {
                transform.DOScale(0f, 0.1f).OnComplete(() => {
                    Destroy(gameObject);
                });
            });
        });
    }

    public void DestroyStartTarget()
    {
        redParticle.gameObject.SetActive(true);
        targetHinge.transform.DORotate(targetHinge.transform.eulerAngles + new Vector3(-90, 0, 0), .3f).OnComplete(() => {
            transform.DOScale(0.35f, 0.1f).OnComplete(() => {
                transform.DOScale(0f, 0.1f).OnComplete(() => {
                    Destroy(gameObject);
                });
            });
        });
    }

    public void DestroyButtonTarget()
    {
        float ActualScale = gameObject.transform.localScale.x;
        redParticle.gameObject.SetActive(true);
        targetHinge.transform.DORotate(targetHinge.transform.eulerAngles + new Vector3(0, 0, 90), .3f).OnComplete(() => {
            transform.DOScale(0.35f, 0.1f).OnComplete(() => {
                transform.DOScale(0f, 0.1f).OnComplete(() => {
                    GetComponent<TargetButton>().OnShoot();
                    targetHinge.transform.DORotate(targetHinge.transform.eulerAngles + new Vector3(0, 0, -90), .3f).OnComplete(() =>
                    {
                        transform.DOScale(ActualScale, 0.1f);
                        });
                    });
            });
        });
    }

    public IEnumerator PauseGame(float pauseTime)
    {
        Time.timeScale = 0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1f;
    }
}
