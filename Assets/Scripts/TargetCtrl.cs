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
        GP = FindObjectOfType<GPCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GP.Player.currentScore = GP.Player.currentScore + 100;
        GP.UI.UpdateScore(GP.Player.currentScore);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    public void Update()
    {
        _chrono += Time.deltaTime;
        if (_chrono >= targetData.duration) Destroy(gameObject);
    }
}
