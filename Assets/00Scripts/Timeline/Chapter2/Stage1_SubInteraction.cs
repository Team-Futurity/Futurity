using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Stage1_SubInteraction : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private PlayableDirector cutScene;
	
	[Header("스크립트 데이터")]
	[SerializeField] private List<ScriptingList> scriptingList;
	[SerializeField] private List<int> firstName;
	private int enableIndex;

	protected override void Init()
	{

	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		chapterManager.scripting.EnableNameText(firstName[enableIndex]);
	}

	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
	}
	
	public void C2A1_Scripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptingList[enableIndex].scriptList);
	}

	public void SetIndex(int index) => enableIndex = index;
}
