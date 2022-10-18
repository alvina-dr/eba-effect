using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICtrl : MonoBehaviour
{
    GPCtrl GP;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;

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
}
