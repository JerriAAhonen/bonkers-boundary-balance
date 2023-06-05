using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighscoreController
{
	public static readonly string HighScoreKey = "HighScore";

	// TODO: Sync this with the main menu UI
	private static readonly int maxHighscoreCount = 10;

	/// <summary>
	/// Updates highscores
	/// </summary>
	/// <param name="score"></param>
	/// <returns>True if the score was a new highscore</returns>
	public static bool OnGameOver(int score)
    {
		// Gather all scores from Player Prefs
		var scores = new List<int>();
		var index = 0;
		var key = $"{HighScoreKey}{index}";
		while (PlayerPrefs.HasKey(key))
		{
			scores.Add(PlayerPrefs.GetInt(key));

			index++;
			key = $"{HighScoreKey}{index}";
		}

		// Add new score to list, and sort so that the best score is first
		scores.Add(score);
		scores.Sort((a, b) => b.CompareTo(a));
		
		// Trim excess scores
		if (scores.Count > maxHighscoreCount)
		{
			var excess = scores.Count - maxHighscoreCount;
			scores.RemoveRange(10, excess);
		}

		// Save new scores list to Player Prefs
		for (int i = 0; i < Mathf.Min(scores.Count, maxHighscoreCount); i++)
		{
			key = $"{HighScoreKey}{i}";
			PlayerPrefs.SetInt(key, scores[i]);
		}

		// This counts a tie as a new highscore
		return scores[0] == score && score > 0;
    }

	/// <summary>
	/// Get savesd score at index "index"
	/// </summary>
	/// <param name="index"></param>
	/// <returns>score if exists, otherwise -1</returns>
	public static int GetHighscore(int index)
	{
		var key = $"{HighScoreKey}{index}";
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetInt(key);
		}
		else
			return -1;
	}
}
