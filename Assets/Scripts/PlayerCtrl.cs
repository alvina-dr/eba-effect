using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float throwPower;
    public float throwUpwardPower;
    public InputActionReference rightTrigger = null;
    public Vector3 throwDirection;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Throw();
        //}
        //if (OculusInputSystem.)
        //{
        //    Throw();
        //}
        //MoveCamera();
    }

    public void OnShoot()
    {
        Throw();
    }

    public void OnAim(InputValue input)
    {
        throwDirection = input.Get<Vector3>();
    }


    public void Throw()
    {
        GameObject _projectile = Instantiate(projectilePrefab, transform);
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(Camera.main.transform.forward * throwPower + transform.up * throwUpwardPower, ForceMode.Impulse);
        _projectile.transform.forward = Camera.main.transform.forward;

    }

    public void MoveCamera()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var mousePosition = new Vector3(ray.origin.x*12, ray.origin.y * 12, 0);

        Camera.main.transform.LookAt(mousePosition);
    }
}
