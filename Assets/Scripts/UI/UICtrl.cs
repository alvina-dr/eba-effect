using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UICtrl : MonoBehaviour
{
    GPCtrl GP;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] public EndMenu endMenu;
    [SerializeField] public PauseMenu pauseMenu;
    [SerializeField] public GameObject inGameMenu;
    [SerializeField] Slider healthBar;

    private void Start()
    {
        GP = GPCtrl.instance;
        //scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = _score.ToString();
    }

    public void UpdateCombo(int _combo)
    {
        comboText.text = _combo.ToString();
    }

    public void UpdateLifeBar(int _health)
    {
        healthBar.value = _health;
        if (healthBar.value <= 0) GP.LooseGame();
        if (healthBar.value > 100) healthBar.value = 100;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void OpenPauseMenu()
    {
        if (Time.timeScale != 1 || pauseMenu.transform.localScale != new Vector3(0, 0, 0) || endMenu.transform.localScale == new Vector3(1, 1, 1)) return;
        pauseMenu.gameObject.SetActive(true);
        AudioEngine.instance.musicStream.Pause();
        pauseMenu.transform.DOScale(1, 0.1f).OnComplete(() =>
        {
            Time.timeScale = 0;
        });
    }
}
