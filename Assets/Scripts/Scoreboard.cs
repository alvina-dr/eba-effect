using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Scoreboard : MonoBehaviour
{
    [System.Serializable]
    public class ScoreboardEntry
    {
        public string name;
        public int score;

        public ScoreboardEntry (string _name, int _score)
        {
            name = _name;
            score = _score;
        }
    }

    public class ScoreboardList
    {
        public List<ScoreboardEntry> entries = new List<ScoreboardEntry>();
    }

    private ScoreboardList scoreList = new ScoreboardList();
    public TMP_InputField inputField;
    public GameObject typeNameMenu;
    public GameObject scoreboardMenu;
    public GameObject scoreEntryPrefab;
    public Transform scoreEntryLayout;

    private void Start()
    {
        if (scoreList.entries.Count == 0)
        {
            if (PlayerPrefs.HasKey("scoreboard" + GPCtrl.instance.musicName))
            {
                string json = PlayerPrefs.GetString("scoreboard" + GPCtrl.instance.musicName); // use scoreboard-levelname
                scoreList = JsonUtility.FromJson<ScoreboardList>(json);
            }
        }
    }

    public void AddScoreButton()
    {
        AddScoreToScoreboard(inputField.text, GPCtrl.instance.Player.currentScore);
        typeNameMenu.SetActive(false);
        scoreboardMenu.SetActive(true);
        ShowScoreboard();
    }

    public void AddScoreToScoreboard(string _name, int _score)
    {
        ScoreboardEntry entry = new(_name, _score);
        scoreList.entries.Add(entry);
        SaveScoreboard();
    }

    public void ShowScoreboard()
    {
        for (int i = 0; i < scoreList.entries.Count; i++)
        {
            InstantiateScoreboardEntry(scoreList.entries[i]);
        }
    }

    public void SaveScoreboard()
    {
        scoreList.entries.Sort(SortByScore);
        if (scoreList.entries.Count > DataHolder.instance.GameSettings.scoreboardLength)
            scoreList.entries = scoreList.entries.Take(DataHolder.instance.GameSettings.scoreboardLength).ToList();
        string json = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString("scoreboard" + GPCtrl.instance.musicName, json);
        PlayerPrefs.Save();
    }

    public void InstantiateScoreboardEntry(ScoreboardEntry scoreboardEntry)
    {
        GameObject scoreEntry = Instantiate(scoreEntryPrefab, scoreEntryLayout);
        scoreEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = scoreboardEntry.name;
        scoreEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreboardEntry.score.ToString();
    }

    static int SortByScore(ScoreboardEntry p1, ScoreboardEntry p2)
    {
        return -p1.score.CompareTo(p2.score);
    }
}
