using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum ECutSceneType
{
	NONE,
	INTRO_SCENE,
	CHAPTER1_AREA1_ENTRY,
	CHAPTER1_AREA3_ENTRY,
	CHAPTER1_AREA3_SPAWN_1,
	CHAPTER2_AREA1_ENTRY,
	CHAPTER2_AREA1_INTERACTION_MAIN,
	BOSS_ENTRY,
	BOSS_ENDPHASE,
	BOSS_DEATH,
	PLAYER_DEATH,
	LASTKILL,
	ACTIVE_ALPHA,
	ACTIVE_BETA,
	CHAPTER1_AREA3_SPAWN_2,
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
