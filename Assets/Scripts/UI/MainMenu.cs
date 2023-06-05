using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private enum PageType { Main, Highscores, Settings }
	private static readonly string VolumeKey = "Volume";

	[SerializeField] private CanvasGroup mainPage;
	[SerializeField] private CanvasGroup highscorePage;
	[SerializeField] private CanvasGroup settingsPage;
	[SerializeField] private Button play;
	[SerializeField] private Button highscores;
	[SerializeField] private Button settings;
    [SerializeField] private Button exit;
	[SerializeField] private List<Button> back;
	[Space]
	[SerializeField] private List<TextMeshProUGUI> highscoreLabels;
	[SerializeField] private Slider volumeSlider;

	private void Awake()
	{
		play.onClick.AddListener(OnPlay);
		highscores.onClick.AddListener(OnHighscores);
		settings.onClick.AddListener(OnSettings);
		exit.onClick.AddListener(OnExit);
		
		foreach(var button in back)
			button.onClick.AddListener(OnBack);

		if (PlayerPrefs.HasKey(VolumeKey))
		{
			var volume = PlayerPrefs.GetFloat(VolumeKey);
			volumeSlider.value = volume;
			OnVolumeChanged(volume);
		}

		volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

		ShowPage(PageType.Main);
	}

	private void OnPlay()
	{
		SaveVolumeSetting();
		SceneManager.LoadScene(1);
	}

	private void OnExit()
	{
		SaveVolumeSetting();
		Application.Quit();
	}

	private void OnHighscores()
	{
		for (int i = 0; i < highscoreLabels.Count; i++)
		{
			var score = HighscoreController.GetHighscore(i);
			highscoreLabels[i].text = score > 0
				? score.ToString() 
				: "---";
		}

		ShowPage(PageType.Highscores);
	}

	private void OnSettings()
	{
		ShowPage(PageType.Settings);
	}

	private void OnBack()
	{
		ShowPage(PageType.Main);
	}

	private void OnVolumeChanged(float newValue)
	{
		AudioManager.Instance.SetVolume(Mathf.Log10(newValue) * 20);
	}

	private void SaveVolumeSetting()
	{
		PlayerPrefs.SetFloat(VolumeKey, volumeSlider.value);
	}

	private void ShowPage(PageType pageType)
	{
		SetCanvasGroupActive(mainPage, pageType == PageType.Main);
		SetCanvasGroupActive(highscorePage, pageType == PageType.Highscores);
		SetCanvasGroupActive(settingsPage, pageType == PageType.Settings);
	}

	private void SetCanvasGroupActive(CanvasGroup canvasGroup, bool active)
	{
		canvasGroup.alpha = active ? 1 : 0;
		canvasGroup.interactable = active;
		canvasGroup.blocksRaycasts = active;
	}
}
