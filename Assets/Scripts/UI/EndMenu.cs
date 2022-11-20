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
    [SerializeField] TextMeshProUGUI titleText;

    public void UpdateEndScore(int _endScore)
    {
        endScore = _endScore;
        endScoreText.text = "Score : " + endScore.ToString();
    }

    public void UpdateTotalDestroyed(int _totalDestroyed)
    {
        totalDestroyed = _totalDestroyed;
        totalDestroyedText.text = totalDestroyed.ToString() + " / " + GPCtrl.instance.CSV.targetDataArray.Length;
    }

    public void UpdateMaxCombo(int _maxCombo)
    {
        maxCombo = _maxCombo;
        maxComboText.text = "Max combo : " +  maxCombo.ToString();
    }

    public void UpdateTitle(bool hasWon)
    {
        if (hasWon) titleText.text = "WELL DONE";
        else titleText.text = "GAME OVER";
    }
}
