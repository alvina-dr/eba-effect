using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    GPCtrl GP;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float throwPower;/*{ get; set; }*/
    [SerializeField] float throwUpwardPower;
    public Vector3 throwDirection;
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;
    private LineRenderer rendoRight;
    private LineRenderer rendoLeft;
    public int currentScore = 0;

    private void Start()
    {
        GP = FindObjectOfType<GPCtrl>();
        rendoRight = GP.Vibration.leftController.GetComponent<LineRenderer>();
        rendoLeft = GP.Vibration.rightController.GetComponent<LineRenderer>();
    }
    void Update()
    {
        ShowRaycast(rendoRight, rightController.transform.position, -rightController.transform.forward, 10f);
        ShowRaycast(rendoLeft, leftController.transform.position, -leftController.transform.forward, 10f);
    }

    public void OnShootRight()
    {
        ShootRight();
    }

    public void ShootRight()
    {
        GP.Vibration.SendHaptics(GP.Vibration.rightController);
        GameObject _projectile = Instantiate(projectilePrefab);
        _projectile.transform.position = rightController.transform.position;
        Vector3 _direction = -rightController.transform.forward; 
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(/*Camera.main.transform.forward*/_direction * throwPower  /*rightController.transform.up*/ /* throwUpwardPower*/, ForceMode.Impulse);
        _projectile.transform.forward = -_direction;
    }

    public void OnShootLeft()
    {
        ShootLeft();
    }

    public void ShootLeft()
    {
        GP.Vibration.SendHaptics(GP.Vibration.leftController);
        GameObject _projectile = Instantiate(projectilePrefab);
        _projectile.transform.position = leftController.transform.position;
        Vector3 _direction = -leftController.transform.forward;
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(/*Camera.main.transform.forward*/_direction * throwPower  /*rightController.transform.up*/ /* throwUpwardPower*/, ForceMode.Impulse);
        _projectile.transform.forward = -_direction;
    }

    public void ShowRaycast(LineRenderer rendo, Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);
        Vector3 endPos = targetPos + (direction * length);
        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            endPos = rayHit.point;
        }
        rendo.SetPosition(0, targetPos);
        rendo.SetPosition(1, endPos);
    }
}
