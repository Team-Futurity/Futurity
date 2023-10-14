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
	[SerializeField] private float maxIntensity = 0.6f;
	[SerializeField] private float changeSpeed = 1.5f;
	[SerializeField, ReadOnly(false)] private bool isVignetteActive = false; 
	private Volume volume;
	private Vignette vignette;
	public Vignette Vignette => vignette;

	// Coroutine
	private IEnumerator playerHitEffect;
	private WaitForSeconds waitForSeconds;
	
	public void Awake()
	{
		Init();
	}
	
	#region PlayerAnimationEventFunc
	
	public void CameraShake(float velocity = 0.4f, float duration = 0.2f)
	{
		impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
		impulseSource.GenerateImpulseWithForce(velocity);
	}
	
	#endregion
	
	#region Vignette
	public void StartHitEffectVignette()
	{
		if (isVignetteActive == true)
		{
			StopHitVignette();
		}
		
		playerHitEffect = HitVignette();
		StartCoroutine(playerHitEffect);
	}

	private void StopHitVignette()
	{
		if (playerHitEffect == null)
		{
			return;
		}
		
		StopCoroutine(playerHitEffect);
		isVignetteActive = false;
		vignette.intensity.value = 0;
	}
	
	private IEnumerator HitVignette()
	{
		isVignetteActive = true;
		
		while (vignette.intensity.value < maxIntensity)
		{
			vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, maxIntensity,
				Time.deltaTime * changeSpeed);

			yield return null;
		}

		while (vignette.intensity.value > 0)
		{
			vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, 0,
				Time.deltaTime * changeSpeed);

			yield return null;
		}

		vignette.intensity.value = 0;
		isVignetteActive = false;
	}
	#endregion

	private void Init()
	{
		camBody = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
		originOrthoSize = cam.m_Lens.OrthographicSize;
		
		// vignette
		volume = Camera.main.GetComponent<Volume>();
		if (volume != null && volume.profile.TryGet(out vignette))
		{
			FDebug.Log("Init Success");
		}
	}
	
	
}
