using Cinemachine;
using Spine.Unity;
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
		AREA1_ENTRYCUTSCENE = 0,
		AREA1_EXITCUTSCENE = 1,
		AREA3_ENTRYCUTSCENE = 2,
		LASTKILLCUTSCENE = 3,
		PLYAERDEATHCUTSCENE = 4,
	}
	
	[Header("DebugMode")] 
	[SerializeField] private bool enableDebugMode;
	public bool IsDebugMode => enableDebugMode;
	[SerializeField] private SpawnerManager spawnerManager;
	private const float StartPos = -12.5f;
	
	[Header("Component")]
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private PlayerInput playerInput;
	[SerializeField] private GameObject playerHand;
	public GameObject uiCanvas;
	private PlayerController playerController;
	public PlayerController PlayerController => playerController;

	[Header("스크립트 출력 UI")] 
	public GameObject scriptingUI;
	[SerializeField] private SkeletonGraphic miraeAnimation;
	[SerializeField] private TextMeshProUGUI textInput;
	[SerializeField] private TextMeshProUGUI nameField;
	[SerializeField] private float textOutputDelay = 0.05f;
	[HideInInspector] public bool isEnd = false;
	private bool isInput = false;
	private AnalogGlitchVolume analogGlitch;
	public AnalogGlitchVolume AnalogGlitch => analogGlitch;

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
		playerInput.enabled = false;
		playerHand.SetActive(false);
		
		cameraBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		
		originTarget = playerCamera.m_Follow;
		originOffset = cameraBody.m_TrackedObjectOffset;
		originOrthoSize = playerCamera.m_Lens.OrthographicSize;

		waitForSecondsRealtime = new WaitForSecondsRealtime(textOutputDelay);
		Camera.main.GetComponent<Volume>().profile.TryGet<AnalogGlitchVolume>(out analogGlitch);

		if (enableDebugMode == false)
		{
			return;
		}
		
		cutSceneList[(int)ECutScene.AREA1_ENTRYCUTSCENE].gameObject.SetActive(false);
		playerModelTf.position = new Vector3(StartPos, playerModelTf.position.y, -0.98f);
		spawnerManager.SpawnEnemy();
	}
	
	public void EnableCutScene(ECutScene cutScene)
	{
		if (cutScene == ECutScene.AREA1_ENTRYCUTSCENE)
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

	public void StartPrintingScript(List<ScriptingStruct> scriptsStruct)
	{
		textPrint = PrintingScript(scriptsStruct);
		StartCoroutine(textPrint);
		StartInputCheck();
	}

	private IEnumerator PrintingScript(List<ScriptingStruct> scriptsStruct)
	{
		int index = 0;
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
	
	#endregion
	
	public void SetActiveScriptsUI(bool active)
	{
		scriptingUI.gameObject.SetActive(active);
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

		playerCamera.m_Follow = playerController.transform;
		playerInput.enabled = true;
	}

	public void EnableUI()
	{
		uiCanvas.SetActive(true);
	}
}
