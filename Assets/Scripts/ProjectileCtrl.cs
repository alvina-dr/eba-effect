using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
     [SerializeField] float lifeDuration;
     [SerializeField] float chrono;
     public bool isActive = false;
    [SerializeField] Material redProjectileMaterial;
    [SerializeField] Material blueProjectileMaterial;

    public void SetupProjectile(TargetData.TargetSide _targetSide)
    {
        isActive = true;
        if (_targetSide == TargetData.TargetSide.right) GetComponent<MeshRenderer>().material = blueProjectileMaterial;
        else GetComponent<MeshRenderer>().material = redProjectileMaterial;
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
