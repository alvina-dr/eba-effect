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
        timingIndicator.transform.DOScale(150f, targetData.duration - DataHolder.instance.GameSettings.targetOffset).SetEase(Ease.Linear).OnComplete(() =>
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
            timingIndicator.transform.DOScale(0.0085f, DataHolder.instance.GameSettings.targetOffset).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GetComponent<BoxCollider>().enabled) return;
                transform.SetParent(null);
                GP.rightTargetIndicator.MoveToFirstTarget();
                GP.leftTargetIndicator.MoveToFirstTarget();
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
        int percentage = Mathf.RoundToInt(chrono / (targetData.duration - DataHolder.instance.GameSettings.targetOffset) * 100);
        int _score = Mathf.RoundToInt(DataHolder.instance.GameSettings.maxPointPerTarget * percentage / 100);
        if (targetSide == targetData.targetSide) _score *= DataHolder.instance.GameSettings.goodSideMultiplier;
        scoreText.text = _score.ToString();
        scoreText.transform.DOScale(1.5f, .3f);
        scoreText.transform.DOLocalMove(scoreText.transform.localPosition + new Vector3(10f, Random.Range(-1f, 1f), Random.Range(-1f, 1f)), .3f);
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
        GP.rightTargetIndicator.MoveToFirstTarget();
        GP.leftTargetIndicator.MoveToFirstTarget();        
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
