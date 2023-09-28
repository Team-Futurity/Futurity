using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerCameraEffect : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private CinemachineVirtualCamera cam;
	[ReadOnly(false), SerializeField] private CinemachineFramingTransposer camBody;
	[ReadOnly(false), SerializeField] private  float originOrthoSize;

	[Space(3)] 
	[Header("Time Scale")] 
	[Tooltip("화면 정지 후 복귀 시간"), SerializeField] private float returnTime = 0.2f;
	[Tooltip("최대 슬로우 타임"), SerializeField] private float targetTimeScale = 0.2f;
	
	private const float ORIGIN_TIMESCALE = 1.0f;
	
	[Space(3)]
	[Header("Shake Camera")] 
	[SerializeField] private CinemachineImpulseSource impulseSource;

	[Space(3)] 
	[Header("Vignette")]
	[SerializeField] private float intensity = 0.6f;
	[SerializeField] private float waitTime = 0.2f;
	private Volume volume;
	private Vignette vignette;
	public Vignette Vignette => vignette;

	// Coroutine
	private IEnumerator lerpTimeScale;
	private IEnumerator timeScaleTimer;
	private IEnumerator playerHitEffect;
	private WaitForSeconds waitForSeconds;
	
	public void Awake()
	{
		Init();
	}

	public void CameraShake(float velocity = 0.4f, float duration = 0.2f)
	{
		impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
		impulseSource.GenerateImpulseWithForce(velocity);
	}

	#region TimeSacleEvent
	// 지정된 시간안에 TimeScale을 0으로 만든다.
	public void StartLerpTime(float reachTime)
	{
		lerpTimeScale = LerpTimeScale(reachTime);
		StartCoroutine(lerpTimeScale);
	}

	// 지정된 시간동안 TimeScale을 target으로 만들었다 복귀시킨다.
	public void StartTimeScaleTimer(float target, float duration)
	{
		timeScaleTimer = TimeScaleTimer(target, duration);
		StartCoroutine(timeScaleTimer);
	}

	public void SetTimeScale(float time = 0.2f)
	{
		Time.timeScale = time;
	}

	public void ResetTimeScale()
	{
		Time.timeScale = 1.0f;
	}
	
	private IEnumerator LerpTimeScale(float reachTime)
	{
		float time = 0;

		while (time < reachTime)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, time / reachTime);
			time += Time.unscaledDeltaTime;

			yield return null;
		}

		Time.timeScale = targetTimeScale;
	}

	private IEnumerator TimeScaleTimer(float target, float duration)
	{
		Time.timeScale = target;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = 1.0f;
	}
	#endregion

	#region Vignette
	public void StartHitEffectVignette()
	{
		playerHitEffect = HitVignette();
		StartCoroutine(playerHitEffect);
	}
	
	private IEnumerator HitVignette()
	{
		vignette.intensity.value = intensity;
		yield return waitForSeconds;
		vignette.intensity.value = 0.0f;
	}
	#endregion

	private void Init()
	{
		camBody = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
		originOrthoSize = cam.m_Lens.OrthographicSize;

		waitForSeconds = new WaitForSeconds(waitTime);
		
		// vignette
		volume = Camera.main.GetComponent<Volume>();
		if (volume != null && volume.profile.TryGet(out vignette))
		{
			FDebug.Log("Init Success");
		}
	}
	
	
}
