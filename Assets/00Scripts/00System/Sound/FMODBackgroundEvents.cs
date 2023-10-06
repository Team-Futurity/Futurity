using UnityEngine;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct EventReferenceInScene
{
	public string sceneName;
	public EventReference eventReference;
}

public class FMODBackgroundEvents: Singleton<FMODBackgroundEvents>
{
	[Header("Ambient")]
	[SerializeField] private List<EventReferenceInScene> ambienceSounds;

	[Header("Bacground Music")]
	[SerializeField] private List<EventReferenceInScene> backgroundMusics;

	private Dictionary<string, EventReference> ambienceSoundsBySceneName = new Dictionary<string, EventReference>();
	private Dictionary<string, EventReference> backgroundMusicBySceneName = new Dictionary<string, EventReference>();

	protected override void Awake()
	{
		base.Awake();

		foreach(var ambience in ambienceSounds)
		{
			if (ambienceSoundsBySceneName.ContainsKey(ambience.sceneName)) { continue; }

			ambienceSoundsBySceneName.Add(ambience.sceneName, ambience.eventReference);
		}

		foreach (var music in backgroundMusics)
		{
			if (backgroundMusicBySceneName.ContainsKey(music.sceneName)) { continue; }

			backgroundMusicBySceneName.Add(music.sceneName, music.eventReference);
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
		
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		EventReference ambience = GetAmbience(scene.name);
		EventReference music = GetMusic(scene.name);

		if (!ambience.IsNull) { AudioManager.Instance.RunAmbientSound(ambience); }
		if (!music.IsNull) { AudioManager.Instance.RunBackgroundMusic(music); }
	}

	public EventReference GetAmbience(string sceneName)
	{
		ambienceSoundsBySceneName.TryGetValue(sceneName, out EventReference returnValue);

		return returnValue;
	}

	public EventReference GetMusic(string sceneName)
	{
		backgroundMusicBySceneName.TryGetValue(sceneName, out EventReference returnValue);

		return returnValue;
	}
}
