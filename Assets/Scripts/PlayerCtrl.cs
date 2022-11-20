using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class PlayerCtrl : MonoBehaviour
{
    GPCtrl GP;
    VibrationCtrl Vibration;
    [SerializeField] float throwPower;/*{ get; set; }*/
    [SerializeField] float throwUpwardPower;
    public Vector3 throwDirection;
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;    
    [SerializeField] Animation rightControllerAnimation;
    [SerializeField] Animation leftControllerAnimation;
    public int currentScore = 0;
    public int currentCombo = 0;
    public int maxCombo = 0;
    public int numTargetDestroyed = 0;
    public int health = 50;
    public int scoreMultiplier = 1;
    public AudioClip shootSound;
    [SerializeField] GameObject projectilePrefab;

    [Header("Laser color")]
    [SerializeField] Gradient normalLeftGradient;
    [SerializeField] Gradient normalRightGradient;
    [SerializeField] Gradient hitGradient;

    private void Start()
    {
        if (GPCtrl.instance != null) GP = GPCtrl.instance;
        if (GPCtrl.instance != null && GP.computerMode)
        {
            Camera.main.transform.position -= new Vector3(0f, 1, 0f);
        }
        if (GP != null) GP.UI.UpdateLifeBar(health);
        rightController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = normalRightGradient;
        leftController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = normalLeftGradient;
        Vibration = GetComponent<VibrationCtrl>();
        rightController.transform.parent.GetComponentInChildren<AudioSource>().clip = DataHolder.instance.GameSettings.gunSound;
        leftController.transform.parent.GetComponentInChildren<AudioSource>().clip = DataHolder.instance.GameSettings.gunSound;
    }
    void Update()
    {
        if (GPCtrl.instance == null || (GPCtrl.instance != null && GP.computerMode))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootComputerLeft();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ShootComputerRight();
            }
            if (GPCtrl.instance != null && GP.computerMode)
                MoveCamera();
        }
        if (GPCtrl.instance != null)
        {
            //pause menu
        }

    }

    public void OnShootRight()
    {
        ShootRight();
    }

    public void ShootRight()
    {
        rightController.transform.parent.GetComponentInChildren<AudioSource>().Play();
        transform.DOScale(1, DataHolder.instance.GameSettings.vibrationOffset).OnComplete(() =>
        {
            Vibration.SendHaptics(Vibration.rightController);
        });
        rightControllerAnimation.Play(rightControllerAnimation.clip.name);
        RaycastHit hit;
        ProjectileCtrl _projectile;
        if (GP != null) _projectile = GP.Projectile.GetProjectile();
        else _projectile =  Instantiate(projectilePrefab).GetComponent<ProjectileCtrl>();
        _projectile.SetupProjectile(TargetData.TargetSide.right);
        _projectile.transform.position = rightController.transform.position + rightController.transform.forward.normalized/4*3;
        Vector3 _direction = rightController.transform.forward;
        _projectile.transform.forward = -_direction;
        _projectile.GetComponent<Rigidbody>().AddForce(_direction * throwPower, ForceMode.Impulse);
        rightController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = hitGradient;
        rightController.transform.DOScale(1, 0.1f).OnComplete(() =>
        {
            rightController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = normalRightGradient;
        });
        if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, Mathf.Infinity))
        {
            if (GP == null) hit.transform.GetComponent<TargetCtrl>().DestroyButtonTarget();
            else if (GP.levelState == GPCtrl.LevelState.Before) hit.transform.GetComponent<TargetCtrl>().DestroyStartTarget();
            else hit.transform.GetComponent<TargetCtrl>().DestroyTargetOnHit(TargetData.TargetSide.right);
            //_projectile.DeactivateProjectile();
        }

    }

    public void OnShootLeft()
    {
        ShootLeft();
    }

    public void ShootLeft()
    {
        leftController.transform.parent.GetComponentInChildren<AudioSource>().Play();
        transform.DOScale(1, DataHolder.instance.GameSettings.vibrationOffset).OnComplete(() =>
        {
            Vibration.SendHaptics(Vibration.leftController);
        });
        leftControllerAnimation.Play(leftControllerAnimation.clip.name);
        RaycastHit hit;
        ProjectileCtrl _projectile;
        if (GP != null) _projectile = GP.Projectile.GetProjectile();
        else _projectile = Instantiate(projectilePrefab).GetComponent<ProjectileCtrl>();
        _projectile.SetupProjectile(TargetData.TargetSide.left);
        _projectile.transform.position = leftController.transform.position + leftController.transform.forward.normalized/4*3;
        Vector3 _direction = leftController.transform.forward;
        _projectile.transform.forward = -_direction;
        _projectile.GetComponent<Rigidbody>().AddForce(_direction * throwPower, ForceMode.Impulse);
        leftController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = hitGradient;
        leftController.transform.DOScale(1, 0.1f).OnComplete(() =>
        {
            leftController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = normalLeftGradient;
        });
        if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, Mathf.Infinity))
        {
            if (GP == null) hit.transform.GetComponent<TargetCtrl>().DestroyButtonTarget();
            else if (GP.levelState == GPCtrl.LevelState.Before) hit.transform.GetComponent<TargetCtrl>().DestroyStartTarget();
            else hit.transform.GetComponent<TargetCtrl>().DestroyTargetOnHit(TargetData.TargetSide.left);
            //_projectile.DeactivateProjectile();
        }
    }

    public void ShootComputerLeft()
    {
        AudioEngine.instance.PlayGunSound(DataHolder.instance.GameSettings.gunSound, false);
        ProjectileCtrl _projectile;
        if (GP != null) _projectile = GP.Projectile.GetProjectile();
        else _projectile = Instantiate(projectilePrefab).GetComponent<ProjectileCtrl>();
        _projectile.SetupProjectile(TargetData.TargetSide.left);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, Mathf.Infinity))
        {
            if (GP == null) hit.transform.GetComponent<TargetCtrl>().DestroyButtonTarget();
            else if (GP.levelState == GPCtrl.LevelState.Before) hit.transform.GetComponent<TargetCtrl>().DestroyStartTarget();
            else hit.transform.GetComponent<TargetCtrl>().DestroyTargetOnHit(TargetData.TargetSide.left);
            //_projectile.DeactivateProjectile();
        }
        _projectile.transform.position = transform.position;
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(Camera.main.ScreenPointToRay(Input.mousePosition).direction * throwPower + transform.up * throwUpwardPower, ForceMode.Impulse);
        _projectile.transform.forward = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
    }

    public void ShootComputerRight()
    {
        AudioEngine.instance.PlayGunSound(DataHolder.instance.GameSettings.gunSound, false);
        ProjectileCtrl _projectile;
        if (GP != null) _projectile = GP.Projectile.GetProjectile();
        else _projectile = Instantiate(projectilePrefab).GetComponent<ProjectileCtrl>();
        _projectile.SetupProjectile(TargetData.TargetSide.right);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, Mathf.Infinity))
        {
            if (GP == null) hit.transform.GetComponent<TargetCtrl>().DestroyButtonTarget();
            else if (GP.levelState == GPCtrl.LevelState.Before) hit.transform.GetComponent<TargetCtrl>().DestroyStartTarget();
            else hit.transform.GetComponent<TargetCtrl>().DestroyTargetOnHit(TargetData.TargetSide.right);
            //_projectile.DeactivateProjectile();
        }
        _projectile.transform.position = transform.position;
        _projectile.GetComponentInChildren<Rigidbody>().AddForce(Camera.main.ScreenPointToRay(Input.mousePosition).direction * throwPower + transform.up * throwUpwardPower, ForceMode.Impulse);
        _projectile.transform.forward = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
    }

    public void MoveCamera()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var mousePosition = new Vector3(ray.origin.x *12, ray.origin.y * 6, 0);

        Camera.main.transform.LookAt(mousePosition);
    }
}
