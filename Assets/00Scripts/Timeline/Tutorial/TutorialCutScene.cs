using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	
	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptingList;
	private int curScriptsIndex;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Resume();
		}
	}

	protected override void Init()
	{
		
	}

	protected override void EnableCutScene()
	{
		
	}
	
	protected override void DisableCutScene()
	{
		SceneLoader.Instance.LoadScene("Chapter1-Stage1");
	}
	
	public void Tutorial_Scripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptingList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptingList.Count) ? curScriptsIndex + 1 : 0;
	}

	public void Pause()
	{
		cutScene.Pause();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}

	public void Resume()
	{
		cutScene.Resume();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}
}
