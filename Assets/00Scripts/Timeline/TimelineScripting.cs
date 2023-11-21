using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum EName
{
	MIRAE,
	SONGSARI,
	BOSS
}

public class TimelineScripting : MonoBehaviour
{
	[Header("스크립트 출력 UI")] 
	[SerializeField] private SkeletonGraphic[] skeletons;
	[SerializeField] private TextMeshProUGUI textInput;
	[SerializeField] private GameObject[] nameText;
	[SerializeField] private float textOutputDelay = 0.05f;
	[SerializeField] private float autoWaitingTime = 1.0f;
	[HideInInspector] public bool isEnd = false;
	private bool isInput = false;

	[Header("스크립트 패널 교체")] 
	[SerializeField] private Image scriptsPenal;
	[SerializeField] private Sprite[] scriptsImage;

	[Header("Sound")] 
	[SerializeField] private FMODUnity.EventReference typingSound;
	private EventInstance soundInst;

	[HideInInspector] public bool isSkip = false;
	[HideInInspector] public bool isAuto = false;

	private WaitForSecondsRealtime waitForSecondsRealtime;
	private IEnumerator textPrint;
	private IEnumerator inputCheck;

	private void Start()
	{
		waitForSecondsRealtime = new WaitForSecondsRealtime(textOutputDelay);
		
		soundInst = AudioManager.Instance.CreateInstance(typingSound);
		soundInst.setParameterByName("Time", 0, true);
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
		
		if (index == (int)ScriptingStruct.ENameType.MIRAE)
		{
			scriptsPenal.sprite = scriptsImage[0];
			return;
		}
		
		scriptsPenal.sprite = scriptsImage[1];
	}
	
	public void DisableAllNameObject()
	{
		foreach (GameObject names in nameText)
		{
			if (names != null)
			{
				names.SetActive(false);
			}
		}
	}
	
	public void EnableStandingImg(string imgName)
	{
		int enableIndex = -1;
		
		switch (imgName)
		{
			case "SONGSARI":
			case "SARI":
				enableIndex = (int)EName.SONGSARI;
				break;
			
			case "BOSS":
			case "SUNKYOUNG":
				enableIndex = (int)EName.BOSS;
				break;
			
			default: 
				return;
		}

		for (int i = 1; i < skeletons.Length; ++i)
		{
			if (i == enableIndex)
			{
				skeletons[i].gameObject.SetActive(true);
				continue;
			}
			
			skeletons[i].gameObject.SetActive(false);
		}
	}
	
	private IEnumerator PrintingScript(List<ScriptingStruct> scriptsStruct)
	{
		foreach (ScriptingStruct scripts in scriptsStruct)
		{
			EnableStandingImg(scripts.nameType.ToString()); 
			MiraeEmotionCheck(scripts.miraeExpression);

			if (skeletons[(int)EName.SONGSARI].gameObject.activeSelf == true)
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
				soundInst.start();
				
				if (isInput == true || isSkip == true)
				{
					textInput.text = scripts.scripts;
					isInput = false;
					break;
				}

				yield return waitForSecondsRealtime;
			}

			while (true)
			{
				if (isInput == true || isSkip == true)
				{
					isInput = false;
					break;
				}

				if (isAuto == true)
				{
					yield return new WaitForSeconds(autoWaitingTime);
					break;
				}

				yield return null;
			}
		}

		isEnd = true;
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputChange, true);
	}
	
	#region EmotionCheck
	
	private void MiraeEmotionCheck(ScriptingStruct.EMiraeExpression type)
	{
		const int index = (int)EName.MIRAE;
		
		switch (type)
		{
			case ScriptingStruct.EMiraeExpression.NONE:
				break;
			
			case ScriptingStruct.EMiraeExpression.ANGRY:
				skeletons[index].AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.IDLE:
				skeletons[index].AnimationState.SetAnimation(0, "idle", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.PANIC:
				skeletons[index].AnimationState.SetAnimation(0, "panic", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.SHORT_SURPRISE:
				skeletons[index].AnimationState.SetAnimation(0, "short_surprise", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.SMILE:
				skeletons[index].AnimationState.SetAnimation(0, "smile", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.SURPRISE:
				skeletons[index].AnimationState.SetAnimation(0, "surprise", true);
				break;
			
			case ScriptingStruct.EMiraeExpression.TRUST_ME:
				skeletons[index].AnimationState.SetAnimation(0, "trust_me", true);
				break;
			
			default:
				return;
		}
	}

	private void SariEmotionCheck(ScriptingStruct.ESariExpression type)
	{
		const int index = (int)EName.SONGSARI;
		
		switch (type)
		{
			case ScriptingStruct.ESariExpression.NONE:
				break;
			
			case ScriptingStruct.ESariExpression.ANGRY:
				skeletons[index].AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.ESariExpression.EMBARRASSED:
				skeletons[index].AnimationState.SetAnimation(0, "embarrassed", true);
				break;
			
			case ScriptingStruct.ESariExpression.IDLE:
				skeletons[index].AnimationState.SetAnimation(0, "idle", true);
				break;
			
			case ScriptingStruct.ESariExpression.SURPRISE:
				skeletons[index].AnimationState.SetAnimation(0, "surprise", true);
				break;
			
			case ScriptingStruct.ESariExpression.SAD:
				skeletons[index].AnimationState.SetAnimation(0, "sad", true);
				break;
			
			default:
				return;
		}
	}
	
	private void BossEmotionCheck(ScriptingStruct.EBossExpression type)
	{
		const int index = (int)EName.BOSS;
		
		switch (type)
		{
			case ScriptingStruct.EBossExpression.NONE:
				break;
			
			case ScriptingStruct.EBossExpression.ANGRY:
				skeletons[index].AnimationState.SetAnimation(0, "angry", true);
				break;
			
			case ScriptingStruct.EBossExpression.IDLE:
				skeletons[index].AnimationState.SetAnimation(0, "idle", true);
				break;
			
			case ScriptingStruct.EBossExpression.LAUGH:
				skeletons[index].AnimationState.SetAnimation(0, "laugh", true);
				break;

			default:
				return;
		}
	}
	
	public void ResetEmotion()
	{
		if (skeletons[(int)EName.MIRAE] != null)
		{
			MiraeEmotionCheck(ScriptingStruct.EMiraeExpression.IDLE);
		}

		if (skeletons[(int)EName.SONGSARI] != null && skeletons[(int)EName.SONGSARI].gameObject.activeSelf == true)
		{
			SariEmotionCheck(ScriptingStruct.ESariExpression.IDLE);
		}

		if (skeletons[(int)EName.BOSS] != null && skeletons[(int)EName.BOSS].gameObject.activeSelf == true)
		{
			SariEmotionCheck(ScriptingStruct.ESariExpression.IDLE);
		}
	}
	#endregion
	
	private void InputChange(InputAction.CallbackContext context)
	{
		isInput = true;
	}
}
