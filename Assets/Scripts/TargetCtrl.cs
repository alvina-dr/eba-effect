using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    public TargetData targetData;
    float _chrono;
    private GPCtrl GP;

    private void Start()
    {
        GP = GPCtrl.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        int percentage = Mathf.RoundToInt(_chrono / targetData.duration * 100);
        GP.Player.currentScore += Mathf.RoundToInt(120 * (100-percentage) / 100);
        GP.UI.UpdateScore(GP.Player.currentScore);
        other.GetComponent<ProjectileCtrl>().DeactivateProjectile();
        Destroy(gameObject);
    }

    public void Update()
    {
        _chrono += Time.deltaTime;
        if (_chrono >= targetData.duration) Destroy(gameObject);
    }
}
