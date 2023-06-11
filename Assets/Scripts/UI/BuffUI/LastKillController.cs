using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;


public class LastKillController : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> DeActivePotals;

	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private float zoomInDelayTime = 0.5f;
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

	// 이 메서드는 플레이어가 몬스터에게 마지막 타격을 입혔을 때 호출되어야 합니다
	public void LastKill()
	{
		StartCoroutine(ReleaseTimer());
		motionCoroutine = StartCoroutine(SlowAndZoomInMotion());
	}

	private IEnumerator SlowAndZoomInMotion()
	{
		cameraComponent.orthographicSize = zoomInCameraSize;

		// 슬로우 모션을 시작합니다
		Time.timeScale = slowMotionFactor;

		yield return new WaitForSeconds(zoomInDelayTime * slowMotionFactor);

		Time.timeScale = 1f;

		// 카메라가 플레이어를 줌인하는 동안 대기합니다
		while (cameraComponent.orthographicSize < normalCameraSize)
		{
			cameraComponent.orthographicSize += zoomInSpeed * Time.deltaTime;
			yield return null;
		}


		laskKillEvent?.Invoke();
	}

	private IEnumerator ReleaseTimer()
	{
		yield return new WaitForSeconds(slowMotionTime);

		PotalActive();
		StopCoroutine(motionCoroutine);
		cameraComponent.orthographicSize = normalCameraSize;
		Time.timeScale = 1f;
	}

	/// <summary>
	/// 스크립트가 비활성화될 때 색상을 원상태로 돌립니다.
	/// </summary>
	private void OnDisable()
	{
		if(cameraComponent != null)
		cameraComponent.orthographicSize = normalCameraSize;
		Time.timeScale = 1f;
	}

	public void PotalActive()
	{
		DeActivePotals[0].GetComponent<StageEndController>().isActiveStageEndPortal = true;


		/*for (int i = 0; i < DeActivePotals.Count; i++)
		{
			DeActivePotals[i].GetComponent<StageEndController>().isActiveStageEndPortal = true;
		}*/
	}
}
