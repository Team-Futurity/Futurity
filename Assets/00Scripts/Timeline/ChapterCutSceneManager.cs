using System;
using System.Collections;
using System.Collections.Generic;
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
	public PlayerCamera playerCamera;
	[SerializeField] private GameObject mainUICanvas;
	public TimelineScripting scripting;
	private PlayerController playerController;
	public PlayerController PlayerController => playerController;

	[Header("슬로우 타임")] 
	[SerializeField] [Tooltip("슬로우 모션 도달 시간")] private float timeToSlowMotion;
	[SerializeField] [Tooltip("복귀 시간")] private float recoveryTime;
	[Tooltip("타임 스케일 목표값")] private readonly float targetTimeScale = 0.2f;

	[Header("추적 대상")]
	[SerializeField] private Transform playerModelTf;

	[Header("GrayScale")]
	private GrayScale grayScale = null;
	public GrayScale GrayScale => grayScale;
	
	[Header("Volume Controller(Only use Timeline)")]
	[SerializeField] private float scanLineJitter;
	[SerializeField] private float colorDrift;
	[HideInInspector] public bool isCutScenePlay = false;
	
	private IEnumerator timeSlow;
	private IEnumerator lerpTimeScale;
	private AnalogGlitchVolume analogGlitch;
	
	public void Start()
	{
		if (isIntroScene == false)
		{
			return;
		}

		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void InitManager()
	{
		//TimelineManager.Instance.InitTimelineManager(cutSceneList);
		TimelineManager.Instance.InitCutSceneManager(GetChildCutScene());

		var player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		
		mainCamera.GetComponent<Volume>().profile.TryGet<AnalogGlitchVolume>(out analogGlitch);
		mainCamera.GetComponent<Volume>().profile.TryGet<GrayScale>(out grayScale);
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
	
	public void SetActiveMainUI(bool active) => mainUICanvas.SetActive(active);

	private List<CutSceneBase> GetChildCutScene()
	{
		int count = transform.childCount;
		List<CutSceneBase> result = new List<CutSceneBase>();

		for (int i = 0; i < count; ++i)
		{
			result.Add(transform.GetChild(i).GetComponent<CutSceneBase>());
		}

		return result;
	}

	#region StandingScripts
	
	public void PauseCutSceneUntilScriptsEnd(PlayableDirector cutScene)
	{
		cutScene.Pause();
		StartCoroutine(WaitScriptsEnd(cutScene));
	}

	private IEnumerator WaitScriptsEnd(PlayableDirector cutScene)
	{
		while (scripting.isEnd == false)
		{
			yield return null;
		}
		
		cutScene.Resume();
		scripting.isEnd = false;
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
