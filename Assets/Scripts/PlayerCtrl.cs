using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float throwPower;
    public float throwUpwardPower;
    public Vector3 throwDirection;
    public Vector3 spawnPosition;
    public GameObject rightController;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Throw();
        //}
        //MoveCamera();
    }

    public void OnShoot()
    {
        Throw();
    }

    public void Throw()
    {
        GameObject _projectile = Instantiate(projectilePrefab);
        _projectile.transform.position = rightController.transform.position;
        Vector3 _direction = -rightController.transform.forward; 
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(/*Camera.main.transform.forward*/_direction * throwPower  /*rightController.transform.up*/ /* throwUpwardPower*/, ForceMode.Impulse);
        _projectile.transform.forward = -_direction;

    }
}
