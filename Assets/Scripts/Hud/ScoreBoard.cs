using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TextMeshProUGUI scoreBoardText;
    public GameObject inputCanvas;
    public GameObject top10Canvas;
    [SerializeField] private string playerName;

    private void Start()
    {
        bool fromGameScene = PlayerPrefs.GetInt("FromGameScene", 0) == 1;

        if (fromGameScene)
        {
            inputCanvas.SetActive(true);
            top10Canvas.SetActive(false);
        }
        else
        {
            inputCanvas.SetActive(false);
            top10Canvas.SetActive(true);
        }

        ShowTopScores();

        PlayerPrefs.SetInt("FromGameScene", 0);
        PlayerPrefs.Save();
    }

    public void SetName()
    {
        playerName = nameInput.text;
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        SaveScore(playerName, lastScore);

        inputCanvas.SetActive(false);
        top10Canvas.SetActive(true);

        Time.timeScale = 1f;
        ShowTopScores();
    }

    private void SaveScore(string name, int score)
    {
        List<string> scores = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            string entry = PlayerPrefs.GetString($"Score{i}", null);
            if (!string.IsNullOrEmpty(entry))
                scores.Add(entry);
        }

        scores.Add($"{name}:{score}");

        scores = scores.OrderByDescending(s =>
        {
            string[] split = s.Split(':');
            return int.Parse(split[1]);
        }).Take(10).ToList();

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetString($"Score{i}", scores[i]);
        }

        PlayerPrefs.Save();
    }

    private void ShowTopScores()
    {
        scoreBoardText.text = "TOP 10\n";

        for (int i = 0; i < 10; i++)
        {
            string entry = PlayerPrefs.GetString($"Score{i}", null);
            if (!string.IsNullOrEmpty(entry))
            {
                string[] split = entry.Split(':');
                scoreBoardText.text += $"{i + 1}. {split[0]} - {split[1]}\n";
            }
        }
    }
}
