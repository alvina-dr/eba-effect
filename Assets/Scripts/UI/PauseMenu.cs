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
        Time.timeScale = 1;
        transform.DOScale(0, .3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
