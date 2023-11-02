using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class SubInteractionCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private PlayableDirector cutScene;
	
	[Header("스크립트 데이터")]
	[SerializeField] private List<ScriptingList> scriptingList;
	[SerializeField] private List<int> firstName;
	private int enableIndex;

	[Header("종료 이벤트")]
	[SerializeField] private UnityEvent endEvent;

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
		
		endEvent?.Invoke();
	}
	
	public void SubInteractionScripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptingList[enableIndex].scriptList);
	}

	public void SetIndex(int index) => enableIndex = index;
}
