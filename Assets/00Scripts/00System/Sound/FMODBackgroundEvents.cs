using UnityEngine;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// ���� ���¿��� ���� �ٲ���.
public enum BackgroundPlayableType
{
	None,
	Continuous,
	ChangeVariable
}

[System.Serializable]
public struct EventReferenceInScene
{
	public string sceneName;
	public string prevSceneName;
	public BackgroundPlayableType backgroundPlayableType;
	public ParamRef variable;
	public bool isAMB;
	public EventReference eventReference;
}

public class FMODBackgroundEvents: Singleton<FMODBackgroundEvents>
{
	[Header("Ambient")]
	[SerializeField] private List<EventReferenceInScene> ambienceSounds;

	[Header("Bacground Music")]
	[SerializeField] private List<EventReferenceInScene> backgroundMusics;

	private Dictionary<string, EventReferenceInScene> ambienceSoundsBySceneName = new Dictionary<string, EventReferenceInScene>();
	private Dictionary<string, EventReferenceInScene> backgroundMusicBySceneName = new Dictionary<string, EventReferenceInScene>();

	private string currentSceneName;

	protected override void Awake()
	{
		base.Awake();

		foreach(var ambience in ambienceSounds)
		{
			if (ambienceSoundsBySceneName.ContainsKey(ambience.sceneName)) { continue; }

			ambienceSoundsBySceneName.Add(ambience.sceneName, ambience);
		}

		foreach (var music in backgroundMusics)
		{
			if (backgroundMusicBySceneName.ContainsKey(music.sceneName)) { continue; }

			backgroundMusicBySceneName.Add(music.sceneName, music);
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		EventReferenceInScene? ambience = GetAmbience(scene.name);
		EventReferenceInScene? music = GetMusic(scene.name);

		if (ambience.HasValue && !ambience.Value.eventReference.IsNull) { RunEventRefernceInScene(ambience.Value); }
		if (music.HasValue && !music.Value.eventReference.IsNull) { RunEventRefernceInScene(music.Value); }

		currentSceneName = scene.name;
	}

	private void RunEventRefernceInScene(EventReferenceInScene eventRef)
	{
		switch(eventRef.backgroundPlayableType)
		{
			case BackgroundPlayableType.None:
				if (eventRef.isAMB) { AudioManager.Instance.RunAmbientSound(eventRef.eventReference); }
				else { AudioManager.Instance.RunBackgroundMusic(eventRef.eventReference); }
				break;
			case BackgroundPlayableType.Continuous:
				break;
			case BackgroundPlayableType.ChangeVariable:
				if (eventRef.isAMB) { AudioManager.Instance.SetParameterForAmbientSound(eventRef.variable); }
				else { AudioManager.Instance.SetParameterForBackgroundMusic(eventRef.variable); }
				break;
		}
	}

	public EventReferenceInScene? GetAmbience(string sceneName)
	{
		bool isSuccess = ambienceSoundsBySceneName.TryGetValue(sceneName, out EventReferenceInScene returnValue);

		return isSuccess ? returnValue : null;
	}

	public EventReferenceInScene? GetMusic(string sceneName)
	{
		bool isSuccess = backgroundMusicBySceneName.TryGetValue(sceneName, out EventReferenceInScene returnValue);

		return isSuccess ? returnValue : null;
	}
}
