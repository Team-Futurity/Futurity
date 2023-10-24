using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum ECutSceneType
{
	ACTIVE_ALPHA,
	PLAYER_DEATH,
	LASTKILL,
	INTRO_SCENE,
	AREA1_ENTRY,
	AREA1_REWARD,
	AREA3_ENTRY,
	AREA3_EXIT,
	BOSS_ENTRY
}

[Serializable]
public class CutSceneContainer : SerializationDictionary<ECutSceneType, GameObject> {}

public class TimelineManager : Singleton<TimelineManager>
{
	[Header("런타임 자동 초기화")]
	[ReadOnly(false)] public CutSceneContainer cutSceneContainer = new CutSceneContainer();
	
	public void InitCutSceneManager(List<CutSceneBase> list)
	{
		cutSceneContainer.Clear();

		foreach (CutSceneBase cutScene in list)
		{
			cutSceneContainer.Add(cutScene.CutSceneType, cutScene.gameObject);
		}
		
		FDebug.Log($"Init Done!(Count : {cutSceneContainer.Count})");
	}

	public void EnableCutScene(ECutSceneType type)
	{
		cutSceneContainer.GetValue(type)?.gameObject.SetActive(true);
	}

	public void EnableNonPlayOnAwakeCutScene(ECutSceneType type)
	{
		GameObject cutScene = cutSceneContainer.GetValue(type);

		if (cutScene == null)
		{
			FDebug.LogError("CutScene Not Found");
			return;
		}

		if (cutScene.TryGetComponent(out PlayableDirector director) == true)
		{
			director.Play();
			return;
		}
		
		FDebug.LogError("Playable Director Not Found");
	}
	
}
