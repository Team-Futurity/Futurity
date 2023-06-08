using System.Collections;
using UnityEngine;

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

	void Start()
	{
		// 카메라 컴포넌트와 초기 Size 값 설정
		cameraComponent = cameraTransform.GetComponent<Camera>();
		cameraController = cameraTransform.GetComponent<CameraController>();
		initialSize = cameraComponent.orthographicSize;
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
}
