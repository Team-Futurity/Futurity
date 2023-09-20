using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	
	// Coroutine
	private IEnumerator lerpTimeScale;

	public void Awake()
	{
		Init();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			StartLerpTime(0.2f);
		}
	}

	public void CameraShake(float velocity = 0.4f, float duration = 0.2f)
	{
		impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
		impulseSource.GenerateImpulseWithForce(velocity);
	}

	#region TimeSacleEvent
	public void StartLerpTime(float reachTime)
	{
		lerpTimeScale = LerpTimeScale(reachTime);
		StartCoroutine(lerpTimeScale);
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
	#endregion

	private void Init()
	{
		camBody = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
		originOrthoSize = cam.m_Lens.OrthographicSize;
	}
}
