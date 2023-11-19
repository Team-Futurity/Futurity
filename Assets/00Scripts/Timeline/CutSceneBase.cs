using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class CutSceneBase : MonoBehaviour
{
	public ECutSceneType CutSceneType => cutSceneType;
	
	[Header("런타임 자동 초기화")]
	[SerializeField, ReadOnly(false)] protected ChapterCutSceneManager chapterManager;
	[SerializeField, ReadOnly(false)] protected PlayableDirector cutScene;
	
	[Space(3)]
	[Header("컷씬 타입")]
	[SerializeField] protected ECutSceneType cutSceneType;
	[SerializeField] protected bool isActiveCutScene;

	[Space(3)] [Header("스크립트 데이터")] 
	[SerializeField] private bool isUseScripting;
	[SerializeField] public List<ScriptingList> scriptingList;
	private int curScriptsIndex;

	[Space(3)] 
	[Header("스켈레톤 데이터")] 
	public bool isUseSkeleton;
	[SerializeField] protected Transform skeletonParent;
	protected Queue<SkeletonGraphic> skeletonQueue;

	private bool isCutSceneEnable = false;
	
	protected virtual void Init()
	{
		chapterManager = gameObject.GetComponentInParent<ChapterCutSceneManager>();

		cutScene = isActiveCutScene
			? gameObject.GetComponentInChildren<PlayableDirector>() : gameObject.GetComponent<PlayableDirector>();
		chapterManager.autoSkipButton.InitPlayCutScene(cutScene);

		if (isUseScripting == true)
		{
			curScriptsIndex = 0;	
		}

		if (isUseSkeleton == true)
		{
			InitSkeletonQueue();
		}
	}
	
	protected virtual void EnableCutScene() { }
	
	protected abstract void DisableCutScene();

	protected void StartScripting(int index = -1)
	{
		if (chapterManager.scripting.isSkip == true)
		{
			return;
		}
		
		cutScene.Pause();
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);

		if (index == -1)
		{
			chapterManager.scripting.StartPrintingScript(scriptingList[curScriptsIndex].scriptList);
			curScriptsIndex = (curScriptsIndex + 1 < scriptingList.Count) ? curScriptsIndex + 1 : 0;
		}
		else
		{
			chapterManager.scripting.StartPrintingScript(scriptingList[index].scriptList);
		}
	}
	
	private void OnEnable()
	{
		Init();
		EnableCutScene();

		isCutSceneEnable = true;

		if (InputActionManager.Instance != null)
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		}
	}

	private void OnDisable()
	{
		if (InputActionManager.Instance != null)
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions
				.Player);
		}

		DisableCutScene();

		if (isUseScripting == false)
		{
			return;
		}
		
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		isCutSceneEnable = false;
		
		if (chapterManager.scripting.isAuto == true || chapterManager.scripting.isSkip == true)
		{
			chapterManager.scripting.isAuto = chapterManager.scripting.isSkip = false;
			cutScene.playableGraph.GetRootPlayable(0).SetSpeed(1.0f);
			
			FadeManager.Instance.FadeOut(0.5f);
		}

		TimelineManager.Instance.isCutScenePlaying = false;
	}

	private void InitSkeletonQueue()
	{
		skeletonQueue = new Queue<SkeletonGraphic>();

		for (int i = 0; i < skeletonParent.childCount; ++i)
		{
			if (skeletonParent.GetChild(i).TryGetComponent(out SkeletonGraphic cut) == false)
			{
				continue;
			}
			
			skeletonQueue.Enqueue(cut);
			cut.gameObject.SetActive(false);
		}
	}
}
