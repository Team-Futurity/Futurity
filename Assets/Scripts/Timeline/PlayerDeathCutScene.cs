using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCutScene : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private GameObject[] disableUI;
	[SerializeField] [Tooltip("플레이어 사망시 호출할 Window")] private GameObject deathOpenWindow;
	[SerializeField] [Tooltip("panel을 생성할 canvas")] private Canvas panelCanvas;
	
	[Header("Value")] 
	[Tooltip("슬로우 시간")] private float slowMotionDuration;
	[Tooltip("타임 스케일 목표값")] private readonly float targetTimeScale = 0.2f;
	
	private CinemachineFramingTransposer cameraBody;
	private Vector3 originOffset;
	private IEnumerator timeSlow;
	private IEnumerator gameOverEffect;

	private void Start()
	{
		cameraBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		originOffset = cameraBody.m_TrackedObjectOffset;
	}

	public void EndPlayerDeathCutScene()
	{
		WindowManager.Instance.WindowOpen(deathOpenWindow, panelCanvas.transform, false, 
			Vector2.zero, Vector2.one);
		
		cameraBody.m_TrackedObjectOffset = originOffset;
	}
	
	public void ResetTimeScale()
	{
		Time.timeScale = 1.0f;
	}
	
	public void StartSlowMotion()
	{
		foreach (var ui in disableUI)
		{
			ui.gameObject.SetActive(false);		
		}
		
		timeSlow = TimeSlow();
		StartCoroutine(timeSlow);
	}
	
	private IEnumerator TimeSlow()
	{
		var time = 0.0f;

		while (time < slowMotionDuration)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, time / slowMotionDuration);
			time += Time.unscaledDeltaTime;

			yield return null;
		}

		Time.timeScale = targetTimeScale;
	}
}
