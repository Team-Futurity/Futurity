using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Stage1_EntryCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	[SerializeField] private SpawnerManager spawnerManager;

	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptingList;
	private int curScriptsIndex;

	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		spawnerManager.SpawnEnemy();
		
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
	}

	public void C2A1_Scripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptingList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptingList.Count) ? curScriptsIndex + 1 : 0;
	}
}
