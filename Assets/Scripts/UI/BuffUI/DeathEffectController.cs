using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// 플레이어의 사망 시, 카메라 줌 인 및 슬로우 모션 효과를 관리하는 클래스입니다.
/// </summary>
public class DeathEffectController : MonoBehaviour
{
	public Transform playerTransform;
	public Transform cameraTransform;
	public float zoomInSpeed = 2f;
	public float slowMotionFactor = 0.5f;
	public float SizeClipTarget = 2; // 카메라의 Size 클리핑 플레인 목표 값

	private Camera cameraComponent; // 카메라 컴포넌트
	private CameraController cameraController; // 카메라 컴포넌트
	private float initialSize; // 초기 Size 값

	[SerializeField]
	private WindowOpenController windowOpenController;

	public PostProcessVolume volume; // PostProcessVolume 컴포넌트를 연결
	private ColorGrading colorGrading; // ColorGrading 이펙트

	/// <summary>
	/// 컴포넌트와 초기 Size 값을 설정합니다.
	/// </summary>
	void Start()
	{
		cameraComponent = cameraTransform.GetComponent<Camera>();
		cameraController = cameraTransform.GetComponent<CameraController>();
		initialSize = cameraComponent.orthographicSize;

		windowOpenController = GetComponent<WindowOpenController>();

		volume.profile.TryGetSettings(out colorGrading);
	}

	/// <summary>
	/// 플레이어가 사망했을 때 호출되는 메서드입니다.
	/// 슬로우 모션 및 카메라 줌 인 효과를 적용하고, 게임 오버 창을 엽니다.
	/// </summary>
	public void PlayerDeath()
	{
		Time.timeScale = slowMotionFactor;
		cameraController.enabled = false;

		StartCoroutine(ZoomInAndSlowMotion());

		StartCoroutine(OpenGameOverWindow());
	}

	/// <summary>
	/// 플레이어 사망 시, 카메라 줌 인 및 슬로우 모션 효과를 적용하는 코루틴입니다.
	/// </summary>
	IEnumerator ZoomInAndSlowMotion()
	{
		Debug.Log("PlayerDeath");

		colorGrading.saturation.value = 0f;
		Vector3 initialCameraPos = cameraTransform.position;
		Vector3 targetCameraPos = playerTransform.position;

		float journeyLength = Vector3.Distance(initialCameraPos, targetCameraPos);
		float startTime = Time.time;

		while (cameraTransform.position != targetCameraPos || cameraComponent.orthographicSize != SizeClipTarget)
		{
			float distCovered = (Time.time - startTime) * zoomInSpeed;
			float fractionOfJourney = distCovered / journeyLength;

			if (cameraTransform.position != targetCameraPos)
			{
				cameraTransform.position = Vector3.Lerp(initialCameraPos, targetCameraPos, fractionOfJourney);
			}

			if (cameraComponent.orthographicSize != SizeClipTarget)
			{
				cameraComponent.orthographicSize = Mathf.Lerp(initialSize, SizeClipTarget, fractionOfJourney);
			}

			yield return null;
		}
	}

	/// <summary>
	/// 게임 오버 창을 열고 화면을 페이드 인/아웃하는 코루틴입니다.
	/// </summary>
	IEnumerator OpenGameOverWindow()
	{
		yield return new WaitForSeconds(2f);
		yield return FadeManager.Instance.FadeCoroutineStart(false, 1, Color.black);
		windowOpenController.WindowDeactiveOpen();
		yield return FadeManager.Instance.FadeCoroutineStart(true, 1, Color.black);
	}

	/// <summary>
	/// 스크립트가 비활성화될 때 색상을 원상태로 돌립니다.
	/// </summary>
	void OnDisable()
	{
		colorGrading.saturation.value = 100f;
	}
}
