using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DeathEffectController : MonoBehaviour
{
	public Transform playerTransform;
	public Transform cameraTransform;
	public float zoomInSpeed = 2f;
	public float slowMotionFactor = 0.5f;
	public float SizeClipTarget = -10f; // ī�޶��� Size Ŭ���� �÷��� ��ǥ ��

	private Camera cameraComponent; // ī�޶� ������Ʈ
	private CameraController cameraController; // ī�޶� ������Ʈ
	private float initialSize; // �ʱ� Size ��

	public PostProcessVolume volume; // PostProcessVolume ������Ʈ�� ����
	private ColorGrading colorGrading; // ColorGrading ����Ʈ

	void Start()
	{
		// ī�޶� ������Ʈ�� �ʱ� Size �� ����
		cameraComponent = cameraTransform.GetComponent<Camera>();
		cameraController = cameraTransform.GetComponent<CameraController>();
		initialSize = cameraComponent.orthographicSize;

		// colorGrading ���� �ʱ�ȭ
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
		// ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ �����·� �����ϴ�.
		colorGrading.saturation.value = 100f;
	}
}
