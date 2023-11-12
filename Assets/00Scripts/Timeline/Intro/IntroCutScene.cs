using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroCutScene : CutSceneBase
{
	[Space(6)]
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
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		FadeManager.Instance.FadeOut(fadeInTime);
	}

	protected override void DisableCutScene()
	{
		FadeManager.Instance.FadeIn(fadeOutTime, () =>
		{
			SceneLoader.Instance.LoadScene(nextSceneName, true);
		});
	}

	public void StartPrintText()
	{
		cutScene.Pause();

		StartCoroutine(PrintText());
	}
	
	public void StartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}
	
	private IEnumerator PrintText()
	{
		for (int i = 0; i < inputText[curIndex].Length; ++i)
		{
			textInput[curIndex].text += inputText[curIndex][i];

			yield return new WaitForSeconds(inputDelay);
		}

		curIndex++;
		cutScene.Resume();
	}
	
}
