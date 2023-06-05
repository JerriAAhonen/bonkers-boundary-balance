using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "-SFX", menuName = "AudioEvent")]
public class AudioEvent : ScriptableObject
{
	[SerializeField] private List<AudioClip> clips;
	[SerializeField, Range(0f, 1f)] private float volume = 1f;
	[SerializeField] private float volumeVariance;
	[SerializeField, Range(-3f, 3f)] private float pitch = 1f;
	[SerializeField] private float pitchVariance;
	[SerializeField] private float minInterval;
	[SerializeField] private float delay;

	public AudioClip Clip => GetRandom(clips);
	public float Volume => Mathf.Clamp(Random.Range(volume - volumeVariance, volume + volumeVariance), 0f, 1f);
	public float Pitch => Mathf.Clamp(Random.Range(pitch - pitchVariance, pitch + pitchVariance), -3f, 3f);

	public float MINInterval => minInterval;
	public float Delay => delay;

	private AudioClip GetRandom(List<AudioClip> clips)
	{
		if (clips.Count == 1)
			return clips[0];

		var rand = Random.Range(0, clips.Count);
		return clips[rand];
	}
}