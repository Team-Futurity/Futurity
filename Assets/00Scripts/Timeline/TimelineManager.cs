using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum EChapter1CutScene
{
	AREA1_ENTRYCUTSCENE = 0,
	AREA1_REWARDCUTSCENE = 1,
	AREA1_EXITCUTSCENE = 2,
	AREA3_ENTRYCUTSCENE = 3,
	AREA3_LASTKILL = 4,
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

public class TimelineManager : Singleton<TimelineManager>
{
	[Header("Component")] 
	[SerializeField] private List<GameObject> cutSceneList = new List<GameObject>();
	[SerializeField] private List<GameObject> publicCutScene = new List<GameObject>();
	public List<GameObject> CutSceneList => cutSceneList;

	public void InitTimelineManager(List<GameObject> initCutScene, List<GameObject> publicScene)
	{
		cutSceneList.Clear();
		publicCutScene.Clear();

		foreach (GameObject cutScene in initCutScene)
		{
			cutSceneList.Add(cutScene);
		}

		foreach (GameObject cutScene in publicScene)
		{
			publicCutScene.Add(cutScene);
		}
	}

	public void EnablePublicCutScene(EPublicCutScene cutScene)
	{
		publicCutScene[(int)cutScene].SetActive(true);
	}
	
	public void Chapter1_EnableCutScene(EChapter1CutScene cutScene)
	{
		if (cutScene == EChapter1CutScene.AREA1_ENTRYCUTSCENE)
		{
			cutSceneList[(int)cutScene].GetComponent<PlayableDirector>().Play();
			return;
		}
		
		cutSceneList[(int)cutScene].SetActive(true);
	}

	public void BossStage_EnableCutScene(EBossCutScene cutScene)
	{
		cutSceneList[(int)cutScene].SetActive(true);
	}
}
