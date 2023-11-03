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
	
	// 2D 애니 컷 씬
	private Queue<SkeletonGraphic> cutSceneQueue;
	
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
	}

	protected override void DisableCutScene()
	{
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
	
	public void StartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(introCutScene, cutSceneQueue);
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
	
}
