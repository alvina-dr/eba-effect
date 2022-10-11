using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
     [SerializeField] float lifeDuration;
     [SerializeField] float chrono;
     public bool isActive = false;

    public void SetupProjectile()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public void DeactivateProjectile()
    {
        isActive = false;
        gameObject.SetActive(false);
        chrono = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Update()
    {
        if (isActive)
        {
            chrono += Time.deltaTime;
            if (chrono >= lifeDuration)
            {
                DeactivateProjectile();
            }
        }

    }


}
