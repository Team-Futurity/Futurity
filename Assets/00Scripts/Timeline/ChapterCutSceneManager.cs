using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering; 
using URPGlitch.Runtime.AnalogGlitch;

public class ChapterCutSceneManager : MonoBehaviour
{
	[Header("Intro씬이라면 체크")] 
	[SerializeField] private bool isIntroScene = false;

	[Header("Component")] 
	[SerializeField] private Camera mainCamera;
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private GameObject mainUICanvas;
	public TimelineScripting scripting;
	private PlayerController playerController;
	public PlayerController PlayerController => playerController;
	[HideInInspector] public TimelineManager timelineManager;

	[Header("슬로우 타임")] 
	[SerializeField] [Tooltip("슬로우 모션 도달 시간")] private float timeToSlowMotion;
	[SerializeField] [Tooltip("복귀 시간")] private float recoveryTime;
	[Tooltip("타임 스케일 목표값")] private readonly float targetTimeScale = 0.2f;

	[Header("컷신 목록")] 
	[SerializeField] private CutSceneStruct cutSceneList;
  
	[Header("추적 대상")]
	[SerializeField] private Transform playerModelTf;
	private Transform originTarget;
	
	[Header("GrayScale")]
	private GrayScale grayScale = null;
	public GrayScale GrayScale => grayScale;
	

	[Header("Volume Controller(Only use Timeline)")]
	[SerializeField] private float scanLineJitter;
	[SerializeField] private float colorDrift;
	[HideInInspector] public bool isCutScenePlay = false;

	// reset offset value
	private Vector3 originOffset;
	private float originOrthoSize;

	private CinemachineFramingTransposer cameraBody;
	private IEnumerator timeSlow;
	private IEnumerator lerpTimeScale;
	private WaitForSecondsRealtime waitForSecondsRealtime;
	private AnalogGlitchVolume analogGlitch;

	// test
	private bool isInit = false;
	
	public void Start()
	{
		if (isIntroScene == true)
		{
			cutSceneList.chapterScene[0].SetActive(true);
			return;
		}
		
		InitManager();
	}

	public void InitManager()
	{
		timelineManager = TimelineManager.Instance;
		timelineManager.InitTimelineManager(cutSceneList);

		var player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();

		cameraBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		
		originTarget = playerCamera.m_Follow;
		originOffset = cameraBody.m_TrackedObjectOffset;
		originOrthoSize = playerCamera.m_Lens.OrthographicSize;
		
		mainCamera.GetComponent<Volume>().profile.TryGet<AnalogGlitchVolume>(out analogGlitch);
		mainCamera.GetComponent<Volume>().profile.TryGet<GrayScale>(out grayScale);
		
		waitForSecondsRealtime = new WaitForSecondsRealtime(0.3f);

		isInit = true;
	}

	private void Update()
	{
		if (isCutScenePlay == false)
		{
			return;
		}
		
		analogGlitch.scanLineJitter.value = scanLineJitter;
		analogGlitch.colorDrift.value = colorDrift;
	}
	
	public Vector3 GetTargetPosition(float distance, Vector3 forward = default(Vector3))
	{
		forward = (forward == Vector3.zero) ? playerModelTf.forward : forward;
		
		var offset = distance * forward;
		return playerModelTf.position + offset;
	}

	public void SetActivePlayerInput(bool active)
	{
		if (active == false)
		{
			InputActionManager.Instance.DisableActionMap();	
			return;
		}
		
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}

	public void SetActiveMainUI(bool active)
	{
		mainUICanvas.SetActive(active);
	}
	
	public void EnableUI()
	{
		mainUICanvas.SetActive(true);
	}
	
	#region StandingScripts
	
	public void PauseCutSceneUntilScriptsEnd(PlayableDirector cutScene, List<ScriptingList> list, int scriptsIndex)
	{
		cutScene.Pause();
		StartCoroutine(WaitScriptsEnd(cutScene, list, scriptsIndex));
	}

	private IEnumerator WaitScriptsEnd(PlayableDirector cutScene, List<ScriptingList> list, int scriptsIndex)
	{
		while (scripting.isEnd == false)
		{
			yield return null;
		}
		
		cutScene.Resume();
		scripting.isEnd = false;

		if (scriptsIndex + 1 < list.Count)
		{
			scriptsIndex++;
			yield return waitForSecondsRealtime;
			//scripting.InitNameField(list[scriptsIndex].scriptList[0].name);
		}
	}

	#endregion
	
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
}
