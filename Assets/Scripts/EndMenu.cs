using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour
{
    [SerializeField] int endScore;
    [SerializeField] int maxCombo;
    [SerializeField] int totalDestroyed;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI maxComboText;
    [SerializeField] TextMeshProUGUI totalDestroyedText;

    public void UpdateEndScore(int _endScore)
    {
        endScore = _endScore;
        endScoreText.text = endScore.ToString();
    }

    public void UpdateTotalDestroyed(int _totalDestroyed)
    {
        totalDestroyed = _totalDestroyed;
        totalDestroyedText.text = totalDestroyed.ToString() + " / " + GPCtrl.instance.CSV.targetDataArray.Length;
    }

    public void UpdateMaxCombo(int _maxCombo)
    {
        maxCombo = _maxCombo;
    }
}
