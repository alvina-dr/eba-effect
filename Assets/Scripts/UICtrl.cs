using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    GPCtrl GP;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] public EndMenu endMenu;
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
        if (healthBar.value <= 0) GP.GameOver();
        if (healthBar.value > 100) healthBar.value = 100;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
