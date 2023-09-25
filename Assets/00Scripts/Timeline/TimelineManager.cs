using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;

public class TimelineManager : Singleton<TimelineManager>
{
	public enum ECutScene
	{
		STAGE1_ENTRYCUTSCENE = 0,
		LASTKILLCUTSCENE = 1,
		PLYAERDEATHCUTSCENE = 2,
		STAGE1_EXITCUTSCENE = 3
	}
	
	[Header("Component")]
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private PlayerInput playerInput;
	public GameObject uiCanvas;
	private PlayerController playerController;
	public PlayerController PlayerController => playerController;

	[Header("스크립트 출력 UI")] 
	public GameObject scriptingUI;
	public GameObject[] character;
	[HideInInspector] public bool isEnd = false;
	[SerializeField] private TextMeshProUGUI textInput;
	[SerializeField] private float textOutputDelay = 0.05f;
	private bool isInput = false;
	private AnalogGlitchVolume analogGlitch;
	public AnalogGlitchVolume AnalogGlitch => analogGlitch;
	public enum ECharacter
	{
		Normal = 0,
		Angry = 1,
		Happy = 2
	}
	
	[Header("슬로우 타임")] 
	[SerializeField] [Tooltip("슬로우 모션 도달 시간")] private float timeToSlowMotion;
	[SerializeField] [Tooltip("복귀 시간")] private float recoveryTime;
	[Tooltip("타임 스케일 목표값")] private readonly float targetTimeScale = 0.2f;

	[Header("컷신 목록")] 
	[SerializeField] private GameObject[] cutSceneList;

	[Header("추적 대상")]
	[SerializeField] private Transform playerModelTf;
	private Transform originTarget;
	
	// reset offset value
	private Vector3 originOffset;
	private float originOrthoSize;

	private CinemachineFramingTransposer cameraBody;
	private IEnumerator timeSlow;
	private IEnumerator lerpTimeScale;
	private IEnumerator textPrint;
	private IEnumerator inputCheck;
	private WaitForSecondsRealtime waitForSecondsRealtime;
	
	private void Start()
	{
		var player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();

		cameraBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		originTarget = playerCamera.m_Follow;
		originOffset = cameraBody.m_TrackedObjectOffset;
		originOrthoSize = playerCamera.m_Lens.OrthographicSize;

		waitForSecondsRealtime = new WaitForSecondsRealtime(textOutputDelay);
		Camera.main.GetComponent<Volume>().profile.TryGet<AnalogGlitchVolume>(out analogGlitch);
	}
	
	public void EnableCutScene(ECutScene cutScene)
	{
		if (cutScene == ECutScene.STAGE1_ENTRYCUTSCENE)
		{
			cutSceneList[(int)cutScene].GetComponent<PlayableDirector>().Play();
		}
		
		cutSceneList[(int)cutScene].SetActive(true);
	}

	public void ResetCameraValue()
	{
		cameraBody.m_TrackedObjectOffset = originOffset;
		playerCamera.m_Lens.OrthographicSize = originOrthoSize;
	}

	public void ResetCameraTarget() => playerCamera.m_Follow = playerController.transform;

	public Vector3 GetOffsetVector(float distance, Vector3 forward = default(Vector3))
	{
		forward = (forward == Vector3.zero) ? playerModelTf.forward : forward;
		
		var offset = distance * forward;
		return playerModelTf.position + offset;
	}

	public void ChangeFollowTarget(bool isNewTarget = false, Transform newTarget = null)
	{
		playerCamera.m_Follow = (isNewTarget) ? newTarget : originTarget;
	}

	public void SetActivePlayerInput(bool active)
	{
		playerInput.enabled = active;
	}
	
	#region TimelineSignalFunc
	
	#region TimeScale
	public void ResetTimeScale()
	{
		Time.timeScale = 1.0f;
	}

	public void StartLerpTimeScale()
	{
		lerpTimeScale = LerpTimeScale();
		StartCoroutine(lerpTimeScale);
	}
	
	public void StartSlowMotion()
	{
		timeSlow = TimeSlow();
		StartCoroutine(timeSlow);
	}
	
	private IEnumerator TimeSlow()
	{
		var time = 0.0f;

		while (time < timeToSlowMotion)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, time / timeToSlowMotion);
			time += Time.unscaledDeltaTime;

			yield return null;
		}

		Time.timeScale = targetTimeScale;
	}

	private IEnumerator LerpTimeScale()
	{
		var time = 0.0f;

		while (time < recoveryTime)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, time / recoveryTime);
			time += Time.unscaledDeltaTime;

			yield return null;
		}

		Time.timeScale = 1.0f;
	}
	
	
	#endregion

	#region ScriptingFunc

	public void StartPrintingScript(List<string> textList)
	{
		textPrint = PrintingScript(textList);
		StartCoroutine(textPrint);
		StartInputCheck();
	}

	private IEnumerator PrintingScript(List<string> textList)
	{
		foreach (string textArr in textList)
		{
			foreach (char text in textArr)
			{
				textInput.text += text;

				if (isInput == true)
				{
					textInput.text = textArr;
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
	
	#endregion
	
	public void SetActiveScriptsUI(bool active)
	{
		scriptingUI.gameObject.SetActive(active);
	}

	public void SetActiveCharacter(string type)
	{
		int index = CompareType(type);
		
		for (int i = 0; i < character.Length; ++i)
		{
			if (i == index)
			{
				character[i].SetActive(true);
				continue;
			}
			
			character[i].SetActive(false);
		}
	}

	private int CompareType(string type)
	{
		int result = 0;

		switch (type)
		{
			case "Normal":
				result = 0;
				break;
			
			case "Angry":
				result = 1;
				break;
			
			default:
				result = 2;
				break;
		}
		
		return result;
	}
	
	#endregion
	
	// test signal
	public void PlayerMoveStage()
	{
		SteageMove.Instance.MoveStage(SteageMove.EStageType.STAGE_2);

		playerCamera.m_Follow = originTarget;
		playerInput.enabled = true;
	}

	public void EnableUI()
	{
		uiCanvas.SetActive(true);
	}
}
