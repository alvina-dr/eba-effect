using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICtrl : MonoBehaviour
{
    GPCtrl GP;
    TextMeshProUGUI scoreText;

    private void Start()
    {
        GP = GPCtrl.instance;
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = _score.ToString();
    }


}
