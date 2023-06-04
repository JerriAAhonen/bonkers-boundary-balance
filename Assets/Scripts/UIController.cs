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
    [SerializeField] private GameObject tapToStart;
    [SerializeField] private GameObject tapToRestart;
    [SerializeField] private TextMeshProUGUI pointsLabel;
    [SerializeField] private TextMeshProUGUI gameOverPointsLabel;

    private void Start()
    {
        tapToStart.SetActive(true);
        tapToRestart.SetActive(false);
        pointsLabel.gameObject.SetActive(true);
        gameOverPointsLabel.gameObject.SetActive(false);
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
        gameOverPointsLabel.text = $"Your score: {points}";
        gameOverPointsLabel.gameObject.SetActive(true);
    }
}
