using System;
using System.Collections.Generic;
using UnityEngine;

public enum EChapter1CutScene
{
	AREA1_ENTRYCUTSCENE = 0,
	AREA1_REWARDCUTSCENE = 1,
}

public enum EChapter1_2
{
	AREA2_ENTRYSCENE,
	AREA2_LASTKILL
}

public enum EBossCutScene
{
	BOSS_ENTRYCUTSCENE = 0,
}

public enum EPublicCutScene
{
	LASTKILLCUTSCENE = 0,
	PLYAERDEATHCUTSCENE
}

public enum EActiveCutScene
{
	ACITVE_ALPHA = 0,
}

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
	[SerializeField] private CutSceneStruct cutSceneList = new CutSceneStruct();
	[ReadOnly(false)] public CutSceneContainer cutSceneContainer = new CutSceneContainer();
	
	public List<GameObject> ChapterScene => cutSceneList.chapterScene;
	public List<GameObject> PublicScene => cutSceneList.publicScene;
	public List<GameObject> ActiveScene => cutSceneList.activeScene;
 
	public void InitTimelineManager(CutSceneStruct cutSceneStruct)
	{
		ClearAllCutSceneList();

		foreach (GameObject cutScene in cutSceneStruct.chapterScene)
		{
			cutSceneList.chapterScene.Add(cutScene);
		}

		foreach (GameObject cutScene in cutSceneStruct.publicScene)
		{
			cutSceneList.publicScene.Add(cutScene);
		}

		foreach (GameObject cutScene in cutSceneStruct.activeScene)
		{
			cutSceneList.activeScene.Add(cutScene);
		}
	}

	public void InitCutSceneManager(List<CutSceneBase> list)
	{
		cutSceneContainer.Clear();

		foreach (CutSceneBase cutScene in list)
		{
			cutSceneContainer.Add(cutScene.CutSceneType, cutScene.gameObject);
		}
		
		FDebug.Log($"Init Done!(Count : {cutSceneContainer.Count})");
	}

	public void EnablePublicCutScene(EPublicCutScene cutScene)
	{
		cutSceneList.publicScene[(int)cutScene].SetActive(true);
	}
	
	public void Chapter1_Area1_EnableCutScene(EChapter1CutScene cutScene)
	{
		cutSceneList.chapterScene[(int)cutScene].SetActive(true);
	}

	public void Chapter1_Area2_EnableCutScene(EChapter1_2 type)
	{
		cutSceneList.chapterScene[(int)type].SetActive(true);
	}

	public void BossStage_EnableCutScene(EBossCutScene cutScene)
	{
		cutSceneList.chapterScene[(int)cutScene].SetActive(true);
	}

	public void EnableActiveCutScene(EActiveCutScene cutScene)
	{
		cutSceneList.activeScene[(int)cutScene].SetActive(true);
	}

	private void ClearAllCutSceneList()
	{
		cutSceneList.chapterScene.Clear();
		cutSceneList.publicScene.Clear();
		cutSceneList.activeScene.Clear();
	}
}
