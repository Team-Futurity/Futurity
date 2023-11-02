using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Stage1_MainInteraction : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private PlayableDirector cutScene;
	[SerializeField] private Collider interactionCollider;
	[SerializeField] private GameObject chapterTrigger;
	
	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptingList;
	private int curScriptsIndex;

	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		base.EnableCutScene();
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();

		interactionCollider.enabled = false;
		chapterTrigger.SetActive(true);
	}

	public void C2A1_InteractionScripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptingList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptingList.Count) ? curScriptsIndex + 1 : 0;
	}
}
