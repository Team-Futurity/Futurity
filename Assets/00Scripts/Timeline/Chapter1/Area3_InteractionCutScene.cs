using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class Area3_InteractionCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	[SerializeField] private List<SpawnerManager> spawnerManager;
	[SerializeField] private List<BoxCollider> interactionCol;
	
	[Header("스크립트 데이터")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;
	
	private bool isCutScenePlayed = false;
	private int curSpawnerIndex = -1;
	
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
		interactionCol.ForEach(x => x.enabled = false);
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
		spawnerManager[curSpawnerIndex].SpawnEnemy();
	}

	public void CheckCutScenePlayed(int index)
	{
		if (isCutScenePlayed == false)
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.CHAPTER1_AREA3_SPAWN);
		}

		curSpawnerIndex = index;
		
		spawnerManager[curSpawnerIndex].SpawnEnemy();
	}
}
