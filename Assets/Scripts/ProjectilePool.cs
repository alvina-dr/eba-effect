using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProjectilePool : MonoBehaviour
{
    GPCtrl GP;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] int numToInstantiate;
    public bool buttonBool = false;
    public List<ProjectileCtrl> bulletPool;

    void Start()
    {
        GP = GPCtrl.instance;
    }

    void Update()
    {
        if (buttonBool)
        {
            InstantiatePool();
        }
        buttonBool = false;
    }

    public void InstantiatePool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
            i--;
        }
        bulletPool.Clear();
        for (int i = 0; i < numToInstantiate; i++)
        {
            ProjectileCtrl _bullet = Instantiate(projectilePrefab, transform).GetComponent<ProjectileCtrl>();
            _bullet.isActive = false;
            _bullet.gameObject.SetActive(false);
            bulletPool.Add(_bullet);
        }
    }

    public ProjectileCtrl GetProjectile()
    {
        ProjectileCtrl _projectile = bulletPool.Find(x => !x.isActive).GetComponent<ProjectileCtrl>();
        Debug.Log("this is my projectile name : " + _projectile.name);
        return _projectile;
    }
}
