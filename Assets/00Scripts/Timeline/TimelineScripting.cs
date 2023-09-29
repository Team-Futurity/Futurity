using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimelineScripting : MonoBehaviour
{
	[Header("스크립트 출력 UI")]
	[SerializeField] private SkeletonGraphic miraeAnimation;
	[SerializeField] private TextMeshProUGUI textInput;
	[SerializeField] private TextMeshProUGUI nameField;
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
		StartCoroutine(textPrint);
		StartInputCheck();
	}

	private IEnumerator PrintingScript(List<ScriptingStruct> scriptsStruct)
	{
		foreach (ScriptingStruct scripts in scriptsStruct)
		{
			EmotionCheck(scripts.expressionType);
			nameField.text = scripts.name;

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

			textInput.text = "";
		}

		isEnd = true;
		StopInputCheck();
		textInput.text = "";
	}
	
	private void EmotionCheck(ScriptingStruct.EExpressionType type)
	{
		switch (type)
		{
			case ScriptingStruct.EExpressionType.NONE:
				break;
			
			case ScriptingStruct.EExpressionType.ANGRY:
				miraeAnimation.AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.EExpressionType.NORMAL:
				miraeAnimation.AnimationState.SetAnimation(0, "normal", true);
				break;
			
			case ScriptingStruct.EExpressionType.PANIC:
				miraeAnimation.AnimationState.SetAnimation(0, "panic", true);
				break;
			
			case ScriptingStruct.EExpressionType.SMILE:
				miraeAnimation.AnimationState.SetAnimation(0, "smile", true);
				break;
			
			case ScriptingStruct.EExpressionType.SURPRISE:
				miraeAnimation.AnimationState.SetAnimation(0, "surprise", true);
				break;
			
			case ScriptingStruct.EExpressionType.TRUST_ME:
				miraeAnimation.AnimationState.SetAnimation(0, "trust_me", true);
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
