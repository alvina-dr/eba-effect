using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    public TargetData targetData;
    public float _chrono;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    public void Update()
    {
        _chrono += Time.deltaTime;
        if (_chrono >= targetData.duration) Destroy(gameObject);
    }
}
