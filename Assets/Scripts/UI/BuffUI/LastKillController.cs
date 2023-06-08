using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;


public class LastKillController : MonoBehaviour
{
	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private float zoomInSpeed = 2f;
	[SerializeField]
	private float slowMotionFactor = 0.2f;
	[SerializeField]
	private float slowMotionTime = 3f;
	[SerializeField]
	private float normalCameraSize = 5f;
	[SerializeField]
	private float zoomInCameraSize = 2f;
	[SerializeField]
	private UnityEvent laskKillEvent;

	private Coroutine motionCoroutine;

	private Camera cameraComponent;

	void Start()
	{
		// 카메라 컴포넌트를 가져옵니다
		cameraComponent = cameraTransform.GetComponent<Camera>();
	}

	// 이 메서드는 플레이어가 몬스터에게 마지막 타격을 입었을 때 호출되어야 합니다
	public void LastKill()
	{
		StartCoroutine(ReleaseTimer());
		motionCoroutine = StartCoroutine(SlowAndZoomInMotion());
	}

	private IEnumerator SlowAndZoomInMotion()
	{
		// 슬로우 모션을 시작합니다
		Time.timeScale = slowMotionFactor;

		// 카메라가 플레이어를 줌인하는 동안 대기합니다
		while (cameraComponent.orthographicSize > zoomInCameraSize)
		{
			cameraComponent.orthographicSize -= zoomInSpeed * Time.deltaTime;
			yield return null;
		}

		laskKillEvent?.Invoke();
	}

	private IEnumerator ReleaseTimer()
	{
		yield return new WaitForSeconds(slowMotionTime * slowMotionFactor);

		StopCoroutine(motionCoroutine);
		cameraComponent.orthographicSize = normalCameraSize;
		Time.timeScale = 1f;
	}

	/// <summary>
	/// 스크립트가 비활성화될 때 색상을 원상태로 돌립니다.
	/// </summary>
	private void OnDisable()
	{
		cameraComponent.orthographicSize = normalCameraSize;
		Time.timeScale = 1f;
	}
}
