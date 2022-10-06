using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Time.deltaTime == 3f)
        {
            Destroy(gameObject);
        }
    }
}
