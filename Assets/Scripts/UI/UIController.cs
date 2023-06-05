using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * VCRosdNEUE Font source:
 * https://www.dafont.com/vcrosdneue.font?fpp=200&af=on&l[]=10&l[]=1&text=3+2+1+Go
 */

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject tapToStart;
    [SerializeField] private TextMeshProUGUI pointsLabel;
    [SerializeField] private TextMeshProUGUI gameOverPointsLabel;
    [SerializeField] private TextMeshProUGUI highscoreLabel;
    [SerializeField] private Color noNewHighscoreColor;
	[SerializeField] private Color newHighscoreColor;
    [Header("Buttons")]
    [SerializeField] private Button restart;
    [SerializeField] private Button home;

	private void Start()
    {
        tapToStart.SetActive(true);
        pointsLabel.gameObject.SetActive(true);
        gameOverPointsLabel.gameObject.SetActive(false);
		highscoreLabel.gameObject.SetActive(false);

        restart.gameObject.SetActive(false);
        home.gameObject.SetActive(false);

        restart.onClick.AddListener(OnRestart);
        home.onClick.AddListener(OnHome);
	}

    public void HideTapToStart()
    {
        tapToStart.SetActive(false);
    }

    public void SetScore(int points)
    {
        pointsLabel.text = points.ToString();
    }

    public void ShowFinalScore(int points, bool isHighscore)
    {
        pointsLabel.gameObject.SetActive(false);

		restart.gameObject.SetActive(true);
		home.gameObject.SetActive(true);

        if (isHighscore)
            NewHighscore(points);
        else
            NoHighscore(points);
    }

    private void NewHighscore(int newHighScore)
    {
        highscoreLabel.text = $"New highscore: {newHighScore}!";
        highscoreLabel.color = newHighscoreColor;
        highscoreLabel.transform.position = gameOverPointsLabel.transform.position;
        highscoreLabel.gameObject.SetActive(true);
    }

    private void NoHighscore(int points)
    {
        highscoreLabel.text = $"Highscore: {PlayerPrefs.GetInt($"{HighscoreController.HighScoreKey}0")}";
		highscoreLabel.color = noNewHighscoreColor;
		highscoreLabel.gameObject.SetActive(true);

		gameOverPointsLabel.text = $"Your score: {points}";
		gameOverPointsLabel.gameObject.SetActive(true);
	}

    private void OnRestart()
    {
        GameManager.Instance.RestartGame();
    }

    private void OnHome()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
