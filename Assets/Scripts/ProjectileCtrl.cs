using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    public float lifeDuration;

    void Update()
    {
        if (Time.deltaTime == lifeDuration)
        {
            Destroy(gameObject);
        }
    }
}
