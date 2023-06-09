using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LevelController levelController;
    [SerializeField] private UIController uiController;
    [SerializeField] private float minPlayerDistanceToLevelEdge;

    private bool gameOver;
    private int score;

    public bool GameRunning { get; private set; }

    private void Update()
    {
        // Game not running yet, start game when recieving the first input
        if (!gameOver && !GameRunning && Input.GetMouseButtonDown(0))
            StartGame();

        // Generate more level ahead of the player
        if (levelController.LastPosition.x - playerController.transform.position.x < minPlayerDistanceToLevelEdge)
            levelController.AddSegments(10);

        // While the game is running, update player score
        if (GameRunning)
        {
            score = Mathf.RoundToInt(Mathf.Max(0, playerController.transform.position.x));
            uiController.SetScore(score);
        }
    }

    public void StartGame()
    {
        GameRunning = true;
        playerController.OnStartGame();
        uiController.HideTapToStart();
    }

    public void GameOver()
    {
        gameOver = true;
        GameRunning = false;

        var isNewHighscore = HighscoreController.OnGameOver(score);
        uiController.ShowFinalScore(score, isNewHighscore);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
		SceneManager.LoadScene(0);
	}
}
