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
        //GetComponent<MeshRenderer>().material.DOColor(Color.red, targetData.duration).SetEase(Ease.Linear);
        transform.LookAt(Camera.main.transform);
        transform.rotation *= Quaternion.Euler(0, 90, 0);
        particles = GetComponentInChildren<ParticleSystem>();
        particles.gameObject.SetActive(false);
        if (GP.levelState == GPCtrl.LevelState.Before) return;
        timingIndicator.DOFillAmount(1, targetData.duration).
        /*timingIndicator.transform.DOScale(0.0085f, targetData.duration).*/SetEase(Ease.Linear).OnComplete(() =>
        {
            GP.targetIndicator.MoveToFirstTarget();
            transform.DOScale(0, 0.2f).OnComplete(() => {
                Destroy(gameObject);
                GP.Player.currentCombo = 0;
                GP.Player.scoreMultiplier = 1;
                GP.Player.health -= DataHolder.instance.GameSettings.targetDamage;
                GP.UI.UpdateLifeBar(GP.Player.health);
                GP.UI.UpdateCombo(GP.Player.currentCombo);
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
}
