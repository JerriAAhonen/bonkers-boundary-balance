using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LevelController levelController;
    [SerializeField] private float minPlayerDistanceToLevelEdge;

    public bool GameRunning { get; private set; }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (levelController.LastPosition.x - playerController.transform.position.x < minPlayerDistanceToLevelEdge)
            levelController.AddSegment();
    }

    public void StartGame()
    {
        GameRunning = true;
    }

    public void GameOver()
    {
        GameRunning = false;
    }
}
