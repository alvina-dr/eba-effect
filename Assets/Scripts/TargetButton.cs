using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TargetButton : MonoBehaviour
{

    public UnityEvent onShootEvent;
    public TextAsset levelAsset;
    [SerializeField] float Scalevalue = 20f;


   void Start()
    {
        //transform.DOScale(Scalevalue, 0.1f);

    }

    public void OnShoot()
    {
        onShootEvent.Invoke();
    }

    public void LoadLevel()
    {
        DataHolder.instance.levelToLoad = levelAsset;
        SceneManager.LoadScene("Game");
    }
}
