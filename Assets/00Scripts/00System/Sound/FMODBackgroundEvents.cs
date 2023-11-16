using UnityEngine;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


// 이전 상태에서 뭐만 바꿀지.
public enum BackgroundPlayableType
{
	None,
	Continuous,
	ChangeVariable,
	Stop
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

		if (ambience.HasValue) { RunEventRefernceInScene(ambience.Value); }
		if (music.HasValue) { RunEventRefernceInScene(music.Value); }

		currentSceneName = scene.name;
	}

	private void RunEventRefernceInScene(EventReferenceInScene eventRef)
	{
		switch(eventRef.backgroundPlayableType)
		{
			case BackgroundPlayableType.None:
				if (eventRef.eventReference.IsNull) { return; }

				if (eventRef.isAMB) { AudioManager.Instance.RunAmbientSound(eventRef.eventReference); }
				else { AudioManager.Instance.RunBackgroundMusic(eventRef.eventReference); }
				break;
			case BackgroundPlayableType.Continuous:
				break;
			case BackgroundPlayableType.ChangeVariable:
				if (eventRef.isAMB) { AudioManager.Instance.SetParameterForAmbientSound(eventRef.variable); }
				else { AudioManager.Instance.SetParameterForBackgroundMusic(eventRef.variable); }
				break;
			case BackgroundPlayableType.Stop:
				if (eventRef.isAMB) { AudioManager.Instance.StopAmbientSound(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
				else { AudioManager.Instance.StopBackgroundMusic(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
				currentSceneName = "";
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
