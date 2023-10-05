using Cinemachine;
using System;
using System.Collections;
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
	public GameObject uiCanvas;
	public TimelineScripting scripting;
	private PlayerController playerController;
	public PlayerController PlayerController => playerController;
	
	[Header("다이얼로그")]
	[SerializeField] private GameObject dialogUI;
	[SerializeField] private UIDialogController dialogController;
	public UIDialogController DialogController => dialogController;

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
	private WaitForSecondsRealtime waitForSecondsRealtime;
	
	private AnalogGlitchVolume analogGlitch;
	public AnalogGlitchVolume AnalogGlitch => analogGlitch;
	
	private void Start()
	{
		var player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		playerInput.enabled = false;

		cameraBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		
		originTarget = playerCamera.m_Follow;
		originOffset = cameraBody.m_TrackedObjectOffset;
		originOrthoSize = playerCamera.m_Lens.OrthographicSize;
		
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
	
	public Vector3 GetOffsetVector(float distance, Vector3 forward = default(Vector3))
	{
		forward = (forward == Vector3.zero) ? playerModelTf.forward : forward;
		
		var offset = distance * forward;
		return playerModelTf.position + offset;
	}

	public void SetActivePlayerInput(bool active)
	{
		playerInput.enabled = active;
	}

	public void StartDialog(DialogData data)
	{
		dialogUI.gameObject.SetActive(true);
		
		dialogController.SetDialogData(data);
		dialogController.PlayDialog();
	}

	public void SetActiveDialogUI(bool active)
	{
		dialogUI.gameObject.SetActive(active);
	}

	#region PlayerCamera
	public void ResetCameraTarget() => playerCamera.m_Follow = playerController.transform;
	
	public void ResetCameraValue()
	{
		cameraBody.m_TrackedObjectOffset = originOffset;
		playerCamera.m_Lens.OrthographicSize = originOrthoSize;
	}
	
	public void ChangeFollowTarget(bool isNewTarget = false, Transform newTarget = null)
	{
		playerCamera.m_Follow = (isNewTarget) ? newTarget : originTarget;
	}
	
	#endregion
	
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
