using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TargetCtrl : MonoBehaviour
{
    public TargetData targetData;
    float chrono;
    private GPCtrl GP;
    [SerializeField] private Image timingIndicator;
    bool animationCanLoop = true;
    [SerializeField] GameObject targetHinge;
    ParticleSystem particles;


    private void Start()
    {
        GP = GPCtrl.instance;
        particles = GetComponentInChildren<ParticleSystem>();
        particles.gameObject.SetActive(false);
        transform.rotation *= Quaternion.Euler(0, -90, 0);
        if (GP == null) return;
        transform.LookAt(Camera.main.transform);
        transform.rotation *= Quaternion.Euler(0, -90, 0);

        if (GP.levelState == GPCtrl.LevelState.Before) return;
        if (targetData.targetSide == TargetData.TargetSide.right) transform.rotation *= Quaternion.Euler(0, 180, 0);
        timingIndicator.DOFillAmount(1, targetData.duration - DataHolder.instance.GameSettings.targetOffset).
        /*timingIndicator.transform.DOScale(0.0085f, targetData.duration).*/SetEase(Ease.Linear).OnComplete(() =>
        {
            timingIndicator.DOFillAmount(1, DataHolder.instance.GameSettings.targetOffset).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (!GetComponent<BoxCollider>().enabled) return;
                transform.SetAsLastSibling();
                if (GP.targetPool.transform.childCount == 1) transform.SetParent(null);
                GP.targetIndicator.MoveToFirstTarget();
                transform.DOScale(0, 0.2f).OnComplete(() => {
                    GP.Player.currentCombo = 0;
                    GP.Player.scoreMultiplier = 1;
                    GP.Player.health -= DataHolder.instance.GameSettings.targetDamage;
                    GP.UI.UpdateLifeBar(GP.Player.health);
                    GP.UI.UpdateCombo(GP.Player.currentCombo);
                    Destroy(gameObject);
                    if (GP.lowPassFilter) return;
                    DOTween.To(() => AudioEngine.instance.lowPass.cutoffFrequency, x => AudioEngine.instance.lowPass.cutoffFrequency = x, 0, .22f).SetEase(GP.inLowPass).OnComplete(() => { //shoud last 0,00171875 * bpm
                                                                                                                                                                                            //if (GP.levelState == GPCtrl.LevelState.Over || GP.levelState == GPCtrl.LevelState.Ending) return;
                        DOTween.To(() => AudioEngine.instance.lowPass.cutoffFrequency, x => AudioEngine.instance.lowPass.cutoffFrequency = x, 22000, .22f).SetEase(GP.outLowPass);

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

    public void DestroyTargetOnHit()
    {
        GP.Player.currentCombo++;
        GP.UI.UpdateCombo(GP.Player.currentCombo);
        int percentage = Mathf.RoundToInt(chrono / targetData.duration * 100);
        GP.Player.currentScore += GP.Combo.ApplyMultiplierToScore(Mathf.RoundToInt(DataHolder.instance.GameSettings.maxPointPerTarget * percentage / 100), GP.Player.currentCombo);
        GP.UI.UpdateScore(GP.Player.currentScore);
        GP.Player.health += 5; //hard value need to be variable to tweak later
        GP.UI.UpdateLifeBar(GP.Player.health);
        GP.Player.numTargetDestroyed++;
        if (GP.Player.currentCombo > GP.Player.maxCombo) GP.Player.maxCombo = GP.Player.currentCombo;
        GetComponent<BoxCollider>().enabled = false;
        transform.SetAsLastSibling();
        if (GP.targetPool.transform.childCount == 1) transform.SetParent(null);
        GP.targetIndicator.MoveToFirstTarget();
        particles.gameObject.SetActive(true);
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
        particles.gameObject.SetActive(true);
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
        particles.gameObject.SetActive(true);
        targetHinge.transform.DORotate(targetHinge.transform.eulerAngles + new Vector3(0, 0, 90), .3f).OnComplete(() => {
            transform.DOScale(0.35f, 0.1f).OnComplete(() => {
                transform.DOScale(0f, 0.1f).OnComplete(() => {
                    GetComponent<TargetButton>().OnShoot();
                    Destroy(gameObject);
                });
            });
        });
    }
}
