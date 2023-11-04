using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class Area3_EntryCutScene : CutSceneBase
{
	[Header("컴포넌트")] 
	[SerializeField] private PlayableDirector chapter1Director;
	[SerializeField] private SpawnerManager spawnerManager;

	[Header("텍스트 출력 리스트")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;
	private PlayerController playerController;
	
	protected override void Init()
	{
	}
	
	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.ResetEmotion();
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.SetActiveMainUI(true);
		chapterManager.isCutScenePlay = false;
		
		spawnerManager.SpawnEnemy();
	}
	
	
	public void Area3_PrintScripts()
	{
		chapter1Director.Pause();

		chapterManager.PauseCutSceneUntilScriptsEnd(chapter1Director);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
	
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
}
