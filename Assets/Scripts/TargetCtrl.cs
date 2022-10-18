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
        GetComponent<MeshRenderer>().material.DOColor(Color.red, targetData.duration).SetEase(Ease.Linear);
        timingIndicator.transform.DOScale(0.0085f, targetData.duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(0, 0.2f).OnComplete(() => {
                Destroy(gameObject);
                GP.Player.currentCombo = 0;
                GP.UI.UpdateCombo(GP.Player.currentCombo);
            });
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        int percentage = Mathf.RoundToInt(chrono / targetData.duration * 100);
        GP.Player.currentScore += Mathf.RoundToInt(120 * percentage / 100);
        GP.UI.UpdateScore(GP.Player.currentScore);
        GP.Player.currentCombo++;
        GP.UI.UpdateCombo(GP.Player.currentCombo);
        other.GetComponent<ProjectileCtrl>().DeactivateProjectile();
        transform.DOScale(0, 0.3f).OnComplete(() => {
            Destroy(gameObject);
        });

    }

    public void Update()
    {
        chrono += Time.deltaTime;
    }
}
