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
    public int currentCombo = 0;
    public int numTargetDestroyed = 0;
    public AudioClip shootSound;

    private void Start()
    {
        GP = GPCtrl.instance;
        rendoRight = GP.Vibration.leftController.GetComponent<LineRenderer>();
        rendoLeft = GP.Vibration.rightController.GetComponent<LineRenderer>();
        if (GP.computerMode)
        {
            Camera.main.transform.position -= new Vector3(0f, 1, 0f);
        }
    }
    void Update()
    {
        ShowRaycast(rendoRight, rightController.transform.position, -rightController.transform.forward, 10f);
        ShowRaycast(rendoLeft, leftController.transform.position, -leftController.transform.forward, 10f);
        if (GP.computerMode)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootComputer();
            }
            MoveCamera();
        }

    }

    public void OnShootRight()
    {
        ShootRight();
    }

    public void ShootRight()
    {
        AudioEngine.instance.PlaySound(shootSound, false);
        GP.Vibration.SendHaptics(GP.Vibration.rightController);
        ProjectileCtrl _projectile = GP.Projectile.GetProjectile();
        _projectile.SetupProjectile();
        _projectile.transform.position = rightController.transform.position;
        Vector3 _direction = -rightController.transform.forward;
        _projectile.transform.forward = -_direction;
        _projectile.GetComponent<Rigidbody>().AddForce(_direction * throwPower, ForceMode.Impulse);
    }

    public void OnShootLeft()
    {
        ShootLeft();
    }

    public void ShootLeft()
    {
        AudioEngine.instance.PlaySound(shootSound, false);
        GP.Vibration.SendHaptics(GP.Vibration.leftController);
        ProjectileCtrl _projectile = GP.Projectile.GetProjectile();
        _projectile.SetupProjectile();
        _projectile.transform.position = leftController.transform.position;
        Vector3 _direction = -leftController.transform.forward;
        _projectile.transform.forward = -_direction /*+ new Vector3(0f, 90f, 90f)*/;
        _projectile.GetComponent<Rigidbody>().AddForce(_direction * throwPower, ForceMode.Impulse);
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

    public void ShootComputer()
    {
        ProjectileCtrl _projectile = GP.Projectile.GetProjectile();
        _projectile.SetupProjectile();
        _projectile.transform.position = transform.position;
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(Camera.main.transform.forward * throwPower + transform.up * throwUpwardPower, ForceMode.Impulse);
        _projectile.transform.forward = Camera.main.transform.forward;
    }

    public void MoveCamera()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var mousePosition = new Vector3(ray.origin.x *12, ray.origin.y * 6, 0);

        Camera.main.transform.LookAt(mousePosition);
    }
}
