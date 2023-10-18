using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TimelineScripting : MonoBehaviour
{
	[Header("스크립트 출력 UI")]
	[SerializeField] private SkeletonGraphic miraeAnimation;
	[SerializeField] private SkeletonGraphic sariAnimation;
	[SerializeField] private SkeletonGraphic bossAnimation;
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
		
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputChange, true);
		StartCoroutine(textPrint);
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
				bossAnimation.gameObject.SetActive(false);
				break;
			
			case "BOSS":
				sariAnimation.gameObject.SetActive(false);
				bossAnimation.gameObject.SetActive(true);
				break;
		}
	}

	public void ResetEmotion()
	{
		MiraeEmotionCheck(ScriptingStruct.EMiraeExpression.IDLE);

		if (sariAnimation.gameObject.activeSelf == true)
		{
			SariEmotionCheck(ScriptingStruct.ESariExpression.IDLE);
		}

		if (bossAnimation.gameObject.activeSelf == true)
		{
			SariEmotionCheck(ScriptingStruct.ESariExpression.IDLE);
		}
	}
	
	public void DisableAllNameObject()
	{
		foreach (GameObject names in nameText)
		{
			names.SetActive(false);
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
			else
			{
				BossEmotionCheck(scripts.bossExpression);
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
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputChange, true);
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
	
	private void BossEmotionCheck(ScriptingStruct.EBossExpression type)
	{
		switch (type)
		{
			case ScriptingStruct.EBossExpression.NONE:
				break;
			
			case ScriptingStruct.EBossExpression.ANGRY:
				bossAnimation.AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.EBossExpression.IDLE:
				bossAnimation.AnimationState.SetAnimation(0, "idle", true);
				break;
			
			case ScriptingStruct.EBossExpression.LAUGH:
				bossAnimation.AnimationState.SetAnimation(0, "laugh", true);
				break;

			default:
				return;
		}
	}

	private void InputChange(InputAction.CallbackContext context)
	{
		isInput = true;
	}
}
