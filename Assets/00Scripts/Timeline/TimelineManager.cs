using System.Collections.Generic;
using UnityEngine;

public enum EChapter1CutScene
{
	AREA1_ENTRYCUTSCENE = 0,
	AREA1_REWARDCUTSCENE = 1,
	AREA1_EXITCUTSCENE = 2,
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
	PLYAERDEATHCUTSCENE,
}

public enum EActiveCutScene
{
	ACITVE_ALPHA = 0,	
}

public class TimelineManager : Singleton<TimelineManager>
{
	[Header("런타임 자동 초기화")] 
	[SerializeField] private CutSceneStruct cutSceneList = new CutSceneStruct();
	
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

	private void ClearAllCutSceneList()
	{
		cutSceneList.chapterScene.Clear();
		cutSceneList.publicScene.Clear();
		cutSceneList.activeScene.Clear();
	}
}
