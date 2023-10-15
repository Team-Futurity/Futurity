using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimelineScripting : MonoBehaviour
{
	[Header("스크립트 출력 UI")]
	[SerializeField] private SkeletonGraphic miraeAnimation;
	[SerializeField] private SkeletonGraphic sariAnimation;
	[SerializeField] private GameObject bossAnimation;
	[SerializeField] private TextMeshProUGUI textInput;
	[SerializeField] private GameObject[] nameText;
	[SerializeField] private float textOutputDelay = 0.05f;
	[HideInInspector] public bool isEnd = false;
	private bool isInput = false;

	private WaitForSecondsRealtime waitForSecondsRealtime;
	private IEnumerator textPrint;
	private IEnumerator inputCheck;
	
	private void Start()
	{
		waitForSecondsRealtime = new WaitForSecondsRealtime(textOutputDelay);
	}

	public void StartPrintingScript(List<ScriptingStruct> scriptsStruct)
	{
		textPrint = PrintingScript(scriptsStruct);
		isEnd = false;

		StartCoroutine(textPrint);
		StartInputCheck();
	}

	public void EnableNameText(int index)
	{
		for (int i = 0; i < nameText.Length; ++i)
		{
			if (index == i)
			{
				nameText[i].SetActive(true);
				continue;
			}
			
			nameText[i].SetActive(false);
		}
	}
	
	public void EnableStandingImg(string imgName)
	{
		switch (imgName)
		{
			case "SARI":
				sariAnimation.gameObject.SetActive(true);
				bossAnimation.SetActive(false);
				break;
			
			case "BOSS":
				sariAnimation.gameObject.SetActive(false);
				bossAnimation.SetActive(true);
				break;
		}
	}

	private IEnumerator PrintingScript(List<ScriptingStruct> scriptsStruct)
	{
		foreach (ScriptingStruct scripts in scriptsStruct)
		{
			MiraeEmotionCheck(scripts.miraeExpression);

			if (sariAnimation.gameObject.activeSelf == true)
			{
				SariEmotionCheck(scripts.sariExpression);
			}

			EnableNameText((int)scripts.nameType);
			textInput.text = "";

			foreach (char text in scripts.scripts)
			{
				textInput.text += text;

				if (isInput == true)
				{
					textInput.text = scripts.scripts;
					isInput = false;
					break;
				}

				yield return waitForSecondsRealtime;
			}

			while (true)
			{
				if (isInput == true)
				{
					isInput = false;
					break;
				}

				yield return null;
			}
		}

		isEnd = true;
		StopInputCheck();
	}
	
	private void MiraeEmotionCheck(ScriptingStruct.EMiraeExpression type)
	{
		switch (type)
		{
			case ScriptingStruct.EMiraeExpression.NONE:
				break;
			
			case ScriptingStruct.EMiraeExpression.ANGRY:
				miraeAnimation.AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.IDLE:
				miraeAnimation.AnimationState.SetAnimation(0, "idle", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.PANIC:
				miraeAnimation.AnimationState.SetAnimation(0, "panic", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.SHORT_SURPRISE:
				miraeAnimation.AnimationState.SetAnimation(0, "short_surprise", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.SMILE:
				miraeAnimation.AnimationState.SetAnimation(0, "smile", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.SURPRISE:
				miraeAnimation.AnimationState.SetAnimation(0, "surprise", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.TRUST_ME:
				miraeAnimation.AnimationState.SetAnimation(0, "trust_me", true);
				break;
			
			default:
				return;
		}
	}

	private void SariEmotionCheck(ScriptingStruct.ESariExpression type)
	{
		switch (type)
		{
			case ScriptingStruct.ESariExpression.NONE:
				break;
			
			case ScriptingStruct.ESariExpression.ANGRY:
				sariAnimation.AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.ESariExpression.EMBARRASSED:
				sariAnimation.AnimationState.SetAnimation(0, "embarrassed", true);
				break;
			
			case ScriptingStruct.ESariExpression.IDLE:
				sariAnimation.AnimationState.SetAnimation(0, "idle", true);
				break;
			
			case ScriptingStruct.ESariExpression.SURPRISE:
				sariAnimation.AnimationState.SetAnimation(0, "surprise", true);
				break;
			
			default:
				return;
		}
	}
	
	private void StartInputCheck()
	{
		inputCheck = InputCheck();
		StartCoroutine(inputCheck);
	}

	private void StopInputCheck()
	{
		if (inputCheck != null)
		{
			StopCoroutine(inputCheck);
		}
	}
	
	private IEnumerator InputCheck()
	{
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				isInput = true;
			}

			yield return null;
		}
	}
}
