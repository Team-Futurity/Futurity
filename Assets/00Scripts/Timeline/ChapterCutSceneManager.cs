using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Rendering; 
using URPGlitch.Runtime.AnalogGlitch;
using Animation = Spine.Animation;

public class ChapterCutSceneManager : MonoBehaviour
{
	[Header("Intro씬 혹은 튜토리얼 이라면 체크")] 
	[SerializeField] private bool isIntroScene = false;
	[SerializeField] private bool isTutorialScene = false;

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

	[Header("SkeletonAnimation")] 
	private Queue<SkeletonGraphic> skeletonQueue;
	private IEnumerator skeletonCutScene;
	private bool isInput = false;

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
		if (isTutorialScene == true)
		{
			TimelineManager.Instance.InitCutSceneManager(GetChildCutScene());
			transform.GetChild(0).gameObject.SetActive(true);
		}
		
		if (isIntroScene == false)
		{
			return;
		}

		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void InitManager()
	{
		TimelineManager.Instance.InitCutSceneManager(GetChildCutScene());

		var player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		
		mainCamera.GetComponent<Volume>().profile.TryGet<AnalogGlitchVolume>(out analogGlitch);
		mainCamera.GetComponent<Volume>().profile.TryGet<GrayScale>(out grayScale);
	}

	public void ResetGlitch()
	{
		analogGlitch.scanLineJitter.value = 0;
		analogGlitch.colorDrift.value = 0;
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

	public void SetActiveMainUI(bool active)
	{
		if (mainUICanvas == null)
		{
			return;
		}
		
		var isOkay = TryGetComponent<GaugeDisable>(out var test);
		if(isOkay)
			test.SetGauge(active);
		
		mainUICanvas.SetActive(active);
	}

	private List<CutSceneBase> GetChildCutScene()
	{
		int count = transform.childCount;
		List<CutSceneBase> result = new List<CutSceneBase>();

		for (int i = 0; i < count; ++i)
		{
			if (transform.GetChild(i).TryGetComponent(out CutSceneBase cutScene) == true)
			{
				result.Add(cutScene);
			}
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

	#region SkeletonCutScene
	public void StartSkeletonCutScene(PlayableDirector director, Queue<SkeletonGraphic> queue, UnityAction action = null)
	{
		director.Pause();
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck, true);

		skeletonCutScene = SkeletonCutScene(director, queue, action);
		StartCoroutine(skeletonCutScene);
	}
	
	private IEnumerator SkeletonCutScene(PlayableDirector director, Queue<SkeletonGraphic> queue, UnityAction action)
	{
		SkeletonGraphic skeleton = queue.Dequeue();
		int curAniIndex = 0;
		int maxAniCount = skeleton.Skeleton.Data.Animations.Count;

		while (true)
		{
			skeleton.gameObject.SetActive(true);

			Animation ani = skeleton.Skeleton.Data.Animations.Items[curAniIndex];
			skeleton.AnimationState.SetAnimation(0, ani, false);
			action?.Invoke();
			
			while (isInput == false)
			{
				yield return null;
			}

			if (curAniIndex + 1 < maxAniCount)
			{
				curAniIndex++;
			}
			else
			{
				if (queue.Count <= 0)
				{
					skeleton.gameObject.SetActive(false);
					break;
				}
				
				skeleton.gameObject.SetActive(false);
				skeleton = queue.Dequeue();

				curAniIndex = 0;
				maxAniCount = skeleton.Skeleton.Data.Animations.Count;
			}
			
			isInput = false;
		}
		
		director.Resume();
		isInput = false;
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck);
	}
	
	private void InputCheck(InputAction.CallbackContext context)
	{
		isInput = true;
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
