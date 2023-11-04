using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area3_InteractionCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	[SerializeField] private SpawnerManager[] spawnerManager;
	
	[Header("스크립트 데이터")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;
	
	private bool isCutScenePlayed = false;
	
	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		if (isCutScenePlayed == true)
		{
			return;
		}
		
		isCutScenePlayed = true;
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.ResetEmotion();
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.SetActiveMainUI(true);
	}

	public void Area3_PrintText()
	{
		cutScene.Pause();

		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
	
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}

	public void SpawnEnemy()
	{
		spawnerManager[0].SpawnEnemy();
	}

	public void CheckCutScenePlayed()
	{
		if (isCutScenePlayed == false)
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.CHAPTER1_AREA3_SPAWN);
			return;
		}
		
		spawnerManager[1].SpawnEnemy();
	}
}
