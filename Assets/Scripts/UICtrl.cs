using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UICtrl : MonoBehaviour
{
    GPCtrl GP;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] public EndMenu endMenu;

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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
