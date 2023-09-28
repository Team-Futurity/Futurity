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
	[Header("Shake Camera")] 
	[SerializeField] private CinemachineImpulseSource impulseSource;

	[Space(3)] 
	[Header("Vignette")]
	[SerializeField] private float intensity = 0.6f;
	[SerializeField] private float waitTime = 0.2f;
	private Volume volume;
	private Vignette vignette;
	private DepthOfField dof;
	public Vignette Vignette => vignette;

	// Coroutine
	private IEnumerator playerHitEffect;
	private IEnumerator timeStop;
	private WaitForSeconds waitForSeconds;
	
	public void Awake()
	{
		Init();
	}

	#region PlayerAnimationEventFunc

	public void StartTimeStop(float duration)
	{
		timeStop = TimeStop(duration);
		StartCoroutine(timeStop);
	}
	
	public void CameraShake(float velocity = 0.4f, float duration = 0.2f)
	{
		impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
		impulseSource.GenerateImpulseWithForce(velocity);
	}

	private IEnumerator TimeStop(float duration)
	{
		Time.timeScale = 0.0f;
		dof.active = true;

		yield return new WaitForSecondsRealtime(duration);

		Time.timeScale = 1.0f;
		dof.active = false;
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
			if (volume.profile.TryGet(out dof))
			{
				FDebug.Log("Init Success");
			}
		}
	}
	
	
}
