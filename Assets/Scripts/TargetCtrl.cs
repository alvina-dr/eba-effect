using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    public enum targetType
    {
        Fixed,
        Moving
    }

    public Vector3 startPosition;
    public float spawnTime;
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
