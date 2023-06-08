using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DeathEffectController : MonoBehaviour
{
	public Transform playerTransform;
	public Transform cameraTransform;
	public float zoomInSpeed = 2f;
	public float slowMotionFactor = 0.5f;
	public float SizeClipTarget = -10f; // 카메라의 Size 클리핑 플레인 목표 값

	private Camera cameraComponent; // 카메라 컴포넌트
	private CameraController cameraController; // 카메라 컴포넌트
	private float initialSize; // 초기 Size 값

	public PostProcessVolume volume; // PostProcessVolume 컴포넌트를 연결
	private ColorGrading colorGrading; // ColorGrading 이펙트

	void Start()
	{
		// 카메라 컴포넌트와 초기 Size 값 설정
		cameraComponent = cameraTransform.GetComponent<Camera>();
		cameraController = cameraTransform.GetComponent<CameraController>();
		initialSize = cameraComponent.orthographicSize;

		// colorGrading 값을 초기화
		volume.profile.TryGetSettings(out colorGrading);
	}

	public void PlayerDeath()
	{
		Time.timeScale = slowMotionFactor;
		cameraController.enabled = false;

		StartCoroutine(ZoomInAndSlowMotion());
	}

	IEnumerator ZoomInAndSlowMotion()
	{
		Debug.Log("PlayerDeath");

		colorGrading.saturation.value = 0f;
		Vector3 initialCameraPos = cameraTransform.position;
		Vector3 targetCameraPos = playerTransform.position;

		float journeyLength = Vector3.Distance(initialCameraPos, targetCameraPos);
		float startTime = Time.time;

		while (cameraTransform.position != targetCameraPos)
		{
			float distCovered = (Time.time - startTime) * zoomInSpeed;
			float fractionOfJourney = distCovered / journeyLength;

			cameraTransform.position = Vector3.Lerp(initialCameraPos, targetCameraPos, fractionOfJourney);
			cameraComponent.orthographicSize = Mathf.Lerp(initialSize, SizeClipTarget, fractionOfJourney);

			yield return null;
		}
	}


	void OnDisable()
	{
		// 스크립트가 비활성화될 때 색상을 원상태로 돌립니다.
		colorGrading.saturation.value = 100f;
	}
}
