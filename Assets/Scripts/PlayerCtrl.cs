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
    public int currentScore = 0;
    public int currentCombo = 0;
    public int numTargetDestroyed = 0;
    public AudioClip shootSound;

    private void Start()
    {
        GP = GPCtrl.instance;
        if (GP.computerMode)
        {
            Camera.main.transform.position -= new Vector3(0f, 1, 0f);
        }
    }
    void Update()
    {
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
        RaycastHit hit;
        ProjectileCtrl _projectile = GP.Projectile.GetProjectile();
        _projectile.SetupProjectile();
        _projectile.transform.position = rightController.transform.position;
        Vector3 _direction = rightController.transform.forward;
        _projectile.transform.forward = -_direction;
        _projectile.GetComponent<Rigidbody>().AddForce(_direction * throwPower, ForceMode.Impulse);
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, Mathf.Infinity))
        {
            hit.transform.GetComponent<TargetCtrl>().DestroyTargetOnHit();
            _projectile.DeactivateProjectile();
        }

    }

    public void OnShootLeft()
    {
        ShootLeft();
    }

    public void ShootLeft()
    {
        AudioEngine.instance.PlaySound(shootSound, false);
        GP.Vibration.SendHaptics(GP.Vibration.leftController);
        RaycastHit hit;

        ProjectileCtrl _projectile = GP.Projectile.GetProjectile();
        _projectile.SetupProjectile();
        _projectile.transform.position = leftController.transform.position;
        Vector3 _direction = leftController.transform.forward;
        _projectile.transform.forward = -_direction;
        _projectile.GetComponent<Rigidbody>().AddForce(_direction * throwPower, ForceMode.Impulse);
        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, Mathf.Infinity))
        {
            hit.transform.GetComponent<TargetCtrl>().DestroyTargetOnHit();
            _projectile.DeactivateProjectile();
        }
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
