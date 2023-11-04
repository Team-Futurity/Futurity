using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TutorialCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	
	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptingList;
	private int curScriptsIndex;
	
	[Header("Skeleton Cut Scene")] 
	[SerializeField] private List<SkeletonGraphic> skeletonList;
	private Queue<SkeletonGraphic> skeletonQueue;

	public UnityEvent onPauseEvent;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Resume();
		}
	}

	protected override void Init()
	{
		skeletonQueue = new Queue<SkeletonGraphic>();

		foreach (SkeletonGraphic skeleton in skeletonList)
		{
			skeletonQueue.Enqueue(skeleton);
			skeleton.gameObject.SetActive(false);
		}
	}

	protected override void EnableCutScene()
	{
		
	}
	
	protected override void DisableCutScene()
	{
		FadeManager.Instance.FadeIn(0.8f, () => SceneLoader.Instance.LoadScene("Chapter1-Stage1"));
	}
	
	public void Tutorial_Scripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptingList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptingList.Count) ? curScriptsIndex + 1 : 0;
	}

	public void Tutorial_StartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}

	public void Pause()
	{
		cutScene.Pause();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		
		onPauseEvent?.Invoke();
	}

	public void Resume()
	{
		cutScene.Resume();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}
}
