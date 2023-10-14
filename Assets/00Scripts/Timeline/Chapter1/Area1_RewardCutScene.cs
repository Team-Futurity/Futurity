using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area1_RewardCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector rewardCutScene;
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex;
	
	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActivePlayerInput(false);
		chapterManager.SetActiveMainUI(false);
	}

	public override void DisableCutScene()
	{
		chapterManager.isCutScenePlay = false;
		chapterManager.SetActivePlayerInput(true);
		chapterManager.SetActiveMainUI(true);
	}

	public void Reward_PrintScripts()
	{
		rewardCutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(rewardCutScene, scriptsList, curScriptsIndex);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
}
