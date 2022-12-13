using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void ClosePauseMenu()
    {
        if (Time.timeScale != 0 || transform.localScale != new Vector3(1, 1, 1)) return;
        Time.timeScale = 1;
        AudioEngine.instance.musicStream.Play();
        transform.DOScale(0, .1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        GPCtrl.instance.UI.fadeMaterial.DOFade(1, .3f).OnComplete(() =>
        {
            SceneManager.LoadScene("Game");
        });
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        GPCtrl.instance.UI.fadeMaterial.DOFade(1, .3f).OnComplete(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
