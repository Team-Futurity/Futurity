using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
	public GameObject uiCanvas;
	private PlayerController playerController;
	private PlayerInput playerInput;
	public PlayerController PlayerController => playerController;

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
	private IEnumerator testCoroutine;
	private IEnumerator timeSlow;
	private IEnumerator lerpTimeScale;
	
	private void Start()
	{
		var player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		playerInput = player.GetComponent<PlayerInput>();
		
		cameraBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		originTarget = playerCamera.m_Follow;
		originOffset = cameraBody.m_TrackedObjectOffset;
		originOrthoSize = playerCamera.m_Lens.OrthographicSize;
		
		// 컷신을 재생하는 함수가 다른곳에서 불릴 때까지 해당 지점에서 1구역 진입 연출을 시작합니다.
		// 로딩이 끝난 후 실행할 수 있도록 의도적으로 함수 실행시간을 지연시킵니다.
		// Time.timeScale = 0.0f;
		// testCoroutine = DelayCutScene(ECutScene.STAGE1_ENTRYCUTSCENE);
		// StartCoroutine(testCoroutine);
	}
	
	public void EnableCutScene(ECutScene cutScene)
	{
		cutSceneList[(int)cutScene].SetActive(true);
	}

	public void ResetCameraValue()
	{
		cameraBody.m_TrackedObjectOffset = originOffset;
		playerCamera.m_Lens.OrthographicSize = originOrthoSize;
	}

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
	
	private IEnumerator DelayCutScene(ECutScene cutScene)
	{
		yield return new WaitForSecondsRealtime(5.0f);
		EnableCutScene(cutScene);
	}

	#region TimelineSignalFunc
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
}
