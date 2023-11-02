using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area1_RewardCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector rewardCutScene;
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private List<ScriptingList> scriptsList;
	
	private int curScriptsIndex;
	
	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActiveMainUI(false);
		
		GameObject.FindWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		chapterManager.isCutScenePlay = false;
		chapterManager.SetActiveMainUI(true);
		spawnerManager.SpawnEnemy();
	}

	public void Reward_PrintScripts()
	{
		rewardCutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(rewardCutScene);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
}
