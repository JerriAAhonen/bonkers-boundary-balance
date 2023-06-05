using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioClip music;
	[SerializeField] private float musicVolume;
	[SerializeField] private Transform sourceContainer;
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioMixerGroup audioMixerGroup;

	private AudioSource musicSource;
	private IObjectPool<AudioSource> pool;
	private readonly List<AudioSource> activeSources = new();
	private readonly Dictionary<AudioEvent, float> cooldowns = new();

	protected override void Awake()
	{
		base.Awake();

		pool = new ObjectPool<AudioSource>(
			CreateAudioSource,
			s =>
			{
				s.gameObject.SetActive(true);
				activeSources.Add(s);
			},
			s =>
			{
				s.Stop();
				s.clip = null;
				s.gameObject.SetActive(false);
				s.transform.SetParent(sourceContainer);
				activeSources.Remove(s);
			},
			Destroy);

		musicSource = CreateAudioSource();
		musicSource.clip = music;
		musicSource.volume = musicVolume;
		musicSource.loop = true;
		musicSource.transform.SetParent(sourceContainer);
		musicSource.transform.localPosition = Vector3.zero;
		musicSource.outputAudioMixerGroup = audioMixerGroup;
		musicSource.Play();
	}

	private void LateUpdate()
	{
		foreach (var s in activeSources)
		{
			if (!s.isPlaying)
			{
				pool.Release(s);
				return;
			}
		}
	}

	public void PlayOnce(AudioEvent audioEvent)
	{
		if (!CanBePlayed(audioEvent))
			return;

		var s = pool.Get();
		s.clip = audioEvent.Clip;
		s.volume = audioEvent.Volume;
		s.pitch = audioEvent.Pitch;
		s.loop = false;
		s.outputAudioMixerGroup = audioMixerGroup;

		if (audioEvent.Delay <= 0f)
			s.Play();
		else
			s.PlayDelayed(audioEvent.Delay);
	}

	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("Volume", volume);
	}

	private AudioSource CreateAudioSource()
	{
		var go = new GameObject("AudioSource");
		go.transform.SetParent(sourceContainer);
		go.transform.localPosition = Vector3.zero;

		var source = go.AddComponent<AudioSource>();
		source.playOnAwake = false;
		source.spatialize = false;
		source.spatialBlend = 0f;
		return source;
	}

	private bool CanBePlayed(AudioEvent ae)
	{
		if (ae.MINInterval <= 0f)
			return true;

		if (cooldowns.TryGetValue(ae, out float endsAt) && Time.realtimeSinceStartup < endsAt)
			return false;

		cooldowns[ae] = Time.realtimeSinceStartup + ae.MINInterval;
		return true;
	}
}
