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

    private void Start()
    {
        GP = GPCtrl.instance;
        //GetComponent<MeshRenderer>().material.DOColor(Color.red, targetData.duration).SetEase(Ease.Linear);
        transform.LookAt(Camera.main.transform);
        transform.rotation *= Quaternion.Euler(0, 90, 0);
        if (GP.levelState == GPCtrl.LevelState.Before) return;
        timingIndicator.DOFillAmount(1, targetData.duration).
        /*timingIndicator.transform.DOScale(0.0085f, targetData.duration).*/SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(0, 0.2f).OnComplete(() => {
                Destroy(gameObject);
                GP.Player.currentCombo = 0;
                GP.Player.health -= 10;
                GP.UI.UpdateLifeBar(GP.Player.health);
                GP.UI.UpdateCombo(GP.Player.currentCombo);
            });
        });
    }

    public void DestroyTargetOnHit()
    {
        int percentage = Mathf.RoundToInt(chrono / targetData.duration * 100);
        GP.Player.currentScore += Mathf.RoundToInt(120 * percentage / 100);
        GP.UI.UpdateScore(GP.Player.currentScore);
        GP.Player.currentCombo++;
        GP.Player.health += 5;
        GP.UI.UpdateLifeBar(GP.Player.health);
        GP.Player.numTargetDestroyed++;
        GP.UI.UpdateCombo(GP.Player.currentCombo);
        if (GP.Player.currentCombo > GP.Player.maxCombo) GP.Player.maxCombo = GP.Player.currentCombo;
        GetComponent<BoxCollider>().enabled = false;
        transform.DOScale(0.35f, 0.1f).OnComplete(() => {
            transform.DOScale(0f, 0.1f).OnComplete(() => {
                Destroy(gameObject);
            });
        });
    }

    public void DestroyStartTarget()
    {
        transform.DOScale(0.35f, 0.1f).OnComplete(() => {
            transform.DOScale(0f, 0.1f).OnComplete(() => {
                Destroy(gameObject);
            });
        });
    }

    public void Update()
    {
        chrono += Time.deltaTime;

    }
}
