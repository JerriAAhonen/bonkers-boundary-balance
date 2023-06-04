using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : class
{
	public static T Instance { get; private set; }

	protected virtual void Awake()
	{
		ProcessSingleton();
	}

	protected virtual void OnDestroy()
    {
		Instance = null;
    }

	private void  ProcessSingleton()
	{
		if (!Application.isPlaying) return;

		if (Instance != null)
		{
			Destroy(gameObject); 
			return;
		}

		Instance = this as T;
		if (Instance == null)
			throw new InvalidCastException($"Unable to cast {GetType()} to {typeof(T)}");
	}
}