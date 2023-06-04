using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * VCRosdNEUE Font source:
 * https://www.dafont.com/vcrosdneue.font?fpp=200&af=on&l[]=10&l[]=1&text=3+2+1+Go
 */

public class UIController : MonoBehaviour
{
	private static readonly string HighScoreKey = "HighScore";

    [SerializeField] private GameObject tapToStart;
    [SerializeField] private GameObject tapToRestart;
    [SerializeField] private TextMeshProUGUI pointsLabel;
    [SerializeField] private TextMeshProUGUI gameOverPointsLabel;
    [SerializeField] private TextMeshProUGUI highscoreLabel;
    [SerializeField] private Color noNewHighscoreColor;
	[SerializeField] private Color newHighscoreColor;

	private void Start()
    {
        tapToStart.SetActive(true);
        tapToRestart.SetActive(false);
        pointsLabel.gameObject.SetActive(true);
        gameOverPointsLabel.gameObject.SetActive(false);
		highscoreLabel.gameObject.SetActive(false);
	}

    public void HideTapToStart()
    {
        tapToStart.SetActive(false);
    }

    public void SetScore(int points)
    {
        pointsLabel.text = points.ToString();
    }

    public void ShowTapToRestart()
    {
        tapToRestart.SetActive(true);
    }

    public void ShowFinalScore(int points)
    {
        pointsLabel.gameObject.SetActive(false);
        
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
			var oldHighScore = PlayerPrefs.GetInt(HighScoreKey);
            if (oldHighScore < points)
                NewHighscore(points);
            else
                NoHighscore(points);
		}
        else
            PlayerPrefs.SetInt(HighScoreKey, points);
    }

    private void NewHighscore(int newHighScore)
    {
        highscoreLabel.text = $"New highscore: {newHighScore}!";
        highscoreLabel.color = newHighscoreColor;
        highscoreLabel.transform.position = gameOverPointsLabel.transform.position;
        highscoreLabel.gameObject.SetActive(true);

        PlayerPrefs.SetInt(HighScoreKey, newHighScore);
    }

    private void NoHighscore(int points)
    {
        highscoreLabel.text = $"Highscore: {PlayerPrefs.GetInt(HighScoreKey)}";
		highscoreLabel.color = noNewHighscoreColor;
		highscoreLabel.gameObject.SetActive(true);

		gameOverPointsLabel.text = $"Your score: {points}";
		gameOverPointsLabel.gameObject.SetActive(true);
	}
}
