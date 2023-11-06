using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class BossPhaseEndCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	[SerializeField] private Animator bossController;

	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex;
	
	[Header("Skeleton Cut Scene")] 
	[SerializeField] private List<SkeletonGraphic> skeletonList;
	private Queue<SkeletonGraphic> skeletonQueue;

	[Header("Init Text")] 
	[SerializeField] private TextMeshProUGUI textField;
	[SerializeField] private string initText;
	[SerializeField] private float delayTime = 0.05f;

	private IEnumerator printText;
	private WaitForSeconds waitForSeconds;
	
	protected override void Init()
	{
		skeletonQueue = new Queue<SkeletonGraphic>();
		
		skeletonList.ForEach(x =>
		{
			skeletonQueue.Enqueue(x);
			x.gameObject.SetActive(false);
		});

		waitForSeconds = new WaitForSeconds(delayTime);
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		chapterManager.SetActiveMainUI(true);
	}
	
	public void StartScripting()
	{
		cutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(cutScene);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}

	public void StartStartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}

	public void PlayBossDeathAnimation()
	{
		
	}

	public void StartPrintText()
	{
		textField.text = "";
		cutScene.Pause();

		printText = PrintText();
		StartCoroutine(printText);
	}

	private IEnumerator PrintText()
	{
		foreach (char text in initText)
		{
			textField.text += text;

			yield return waitForSeconds;
		}
		
		cutScene.Resume();
	}
}
