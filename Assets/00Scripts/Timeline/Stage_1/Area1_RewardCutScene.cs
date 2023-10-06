using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area1_RewardCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector rewardCutScene;
	[SerializeField] private List<ScriptingList> scriptsList;

	private TimelineManager manager;
	private int curScriptsIndex;
	
	protected override void Init()
	{
		manager = TimelineManager.Instance;
	}

	protected override void EnableCutScene()
	{
		manager.isCutScenePlay = true;
		manager.SetActivePlayerInput(false);
		manager.SetActiveMainUI(false);
	}

	public override void DisableCutScene()
	{
		manager.isCutScenePlay = false;
		manager.SetActivePlayerInput(true);
		manager.SetActiveMainUI(true);
	}

	public void Reward_PrintScripts()
	{
		rewardCutScene.Pause();
		
		manager.PauseCutSceneUntilScriptsEnd(rewardCutScene, scriptsList, curScriptsIndex);
		manager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
}
