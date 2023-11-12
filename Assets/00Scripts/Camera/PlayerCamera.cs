using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerCamera : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private TimeScaleManager timeScaleManager;
	[ReadOnly(false), SerializeField] private CinemachineVirtualCamera playerCamera;
	[ReadOnly(false), SerializeField] private CinemachineFramingTransposer camBody;
	
	public TimeScaleManager TimeScaleManager => timeScaleManager;

	[Space(3)]
	[Header("Origin Value")]
	[ReadOnly(false), SerializeField] private float originOrthoSize;
	[ReadOnly(false), SerializeField] private Transform originTarget;
	[ReadOnly(false), SerializeField] private Vector3 originOffset;
	
	[Space(3)]
	[Header("Shake Camera")] 
	[SerializeField] private CinemachineImpulseSource impulseSource;

	[Space(3)] 
	[Header("Vignette")]
	[SerializeField] private float maxIntensity = 0.6f;
	[SerializeField] private float changeSpeed = 1.5f;
	[SerializeField, ReadOnly(false)] private bool isVignetteActive = false;

	[Space(3)] 
	[Header("Chromatic Aberration")] 
	[SerializeField] private float fadeOutTime = 0.3f;
	private Volume volume;
	private Vignette vignette;
	private ChromaticAberration chromaticAberration;
	public Vignette Vignette => vignette;

	// Coroutine
	private IEnumerator playerHitEffect;
	private IEnumerator chromaticEffect;
	private WaitForSeconds waitForSeconds;
	
	public void Awake()
	{
		Init();
	}

	#region CameraFunc
	public void ChangeCameraFollowTarget(Transform target) => playerCamera.m_Follow = target;
	public void ResetCameraTarget() => playerCamera.m_Follow = originTarget;
	public void RevertCameraValue()
	{
		camBody.m_TrackedObjectOffset = originOffset;
		playerCamera.m_Lens.OrthographicSize = originOrthoSize;
	}
	#endregion
	
	#region PlayerAnimationEventFunc
	
	public void CameraShake(float velocity = 0.4f, float duration = 0.2f)
	{
		impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
		impulseSource.GenerateImpulseWithForce(velocity);
	}

	public void StartChromaticAberration(float intensity, float duration)
	{
		chromaticEffect = ChromaticAberration(intensity, duration);
		StartCoroutine(chromaticEffect);
	}

	private IEnumerator ChromaticAberration(float intensity, float duration)
	{
		chromaticAberration.intensity.value = intensity;

		yield return new WaitForSeconds(duration);

		float curTime = 0;
		while (chromaticAberration.intensity.value > 0)
		{
			curTime += Time.deltaTime;
			chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0,
				curTime / fadeOutTime);
			
			yield return null;
		}

		chromaticAberration.intensity.value = 0;
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
		playerCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
		camBody = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

		originTarget = GameObject.FindWithTag("Player").transform;
		originOrthoSize = playerCamera.m_Lens.OrthographicSize;
		originOffset = camBody.m_TrackedObjectOffset;
		
		// vignette
		volume = Camera.main.GetComponent<Volume>();
		if (volume != null && volume.profile.TryGet(out vignette) && volume.profile.TryGet(out chromaticAberration))
		{
			FDebug.Log("Init Success");
		}
	}
}
