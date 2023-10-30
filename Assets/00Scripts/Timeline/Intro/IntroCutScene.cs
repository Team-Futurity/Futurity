using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Animation = Spine.Animation;

public class IntroCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector introCutScene;
	[SerializeField] private Transform skeletonParent;
	
	[Header("Fade Time")] 
	[SerializeField] private float fadeInTime = 0.8f;
	[SerializeField] private float fadeOutTime = 0.8f;

	[Header("다음으로 이동할 씬 이름")] 
	[SerializeField] private string nextSceneName;

	[Header("Text 출력")] 
	[SerializeField] private List<TextMeshProUGUI> textInput;
	[SerializeField] private List<string> inputText;
	[SerializeField] private float inputDelay = 0.05f;
	private int curIndex;

	private Queue<SkeletonGraphic> cutSceneQueue;
	private bool isInput = false;
	private bool isInputCheck = false;

	private IEnumerator skeletonCutScene;
	
	protected override void Init()
	{
		cutSceneQueue = new Queue<SkeletonGraphic>();

		for (int i = 0; i < skeletonParent.childCount; ++i)
		{
			cutSceneQueue.Enqueue(skeletonParent.GetChild(i).GetComponent<SkeletonGraphic>());
			skeletonParent.GetChild(i).gameObject.SetActive(false);	
		}
	}

	protected override void EnableCutScene()
	{
		FadeManager.Instance.FadeOut(fadeInTime);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck, true);
	}

	protected override void DisableCutScene()
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck);
		
		FadeManager.Instance.FadeIn(fadeOutTime, () =>
		{
			SceneLoader.Instance.LoadScene(nextSceneName);
		});
	}

	public void StartPrintText()
	{
		introCutScene.Pause();

		StartCoroutine(PrintText());
	}

#if UNITY_EDITOR
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && isInputCheck == true)
		{
			isInput = true;
		}
	}
	#endif

	public void StartSkeletonCutScene()
	{
		PauseTimeline();
		
		skeletonCutScene = SkeletonCutScene();
		StartCoroutine(skeletonCutScene);
	}

	private IEnumerator SkeletonCutScene()
	{
		SkeletonGraphic skeleton = cutSceneQueue.Dequeue();
		int curAniIndex = 0;
		int maxAniCount = skeleton.Skeleton.Data.Animations.Count;

		while (true)
		{
			skeleton.gameObject.SetActive(true);
			isInputCheck = true;
			
			Animation ani = skeleton.Skeleton.Data.Animations.Items[curAniIndex];
			skeleton.AnimationState.SetAnimation(0, ani, false);
			
			while (isInput == false)
			{
				yield return null;
			}

			if (curAniIndex + 1 < maxAniCount)
			{
				curAniIndex++;
			}
			else
			{
				if (cutSceneQueue.Count <= 0 || cutSceneQueue.Count == 3)
				{
					skeleton.gameObject.SetActive(false);
					break;
				}
				
				skeleton.gameObject.SetActive(false);
				skeleton = cutSceneQueue.Dequeue();

				curAniIndex = 0;
				maxAniCount = skeleton.Skeleton.Data.Animations.Count;
			}
			
			isInput = false;
		}
		
		isInputCheck = isInput = false;
		introCutScene.Resume();
	}

	private IEnumerator PrintText()
	{
		for (int i = 0; i < inputText[curIndex].Length; ++i)
		{
			textInput[curIndex].text += inputText[curIndex][i];

			yield return new WaitForSeconds(inputDelay);
		}

		curIndex++;
		introCutScene.Resume();
	}

	private void InputCheck(InputAction.CallbackContext context)
	{
		isInput = true;
	}
	
	private void PauseTimeline()
	{
		introCutScene.Pause();
	}
}
