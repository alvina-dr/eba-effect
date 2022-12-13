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

    public void OnShoot()
    {
        onShootEvent.Invoke();
    }

    public void LoadLevel()
    {
        DataHolder.instance.levelToLoad = levelAsset;
        FindObjectOfType<MainMenu>().fadeMaterial.DOFade(1, .3f).OnComplete(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }
}
