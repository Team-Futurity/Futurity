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
		// ī�޶� ������Ʈ�� �����ɴϴ�
		cameraComponent = cameraTransform.GetComponent<Camera>();
	}

	// �� �޼���� �÷��̾ ���Ϳ��� ������ Ÿ���� ������ �� ȣ��Ǿ�� �մϴ�
	public void LastKill()
	{
		StartCoroutine(ReleaseTimer());
		motionCoroutine = StartCoroutine(SlowAndZoomInMotion());
	}

	private IEnumerator SlowAndZoomInMotion()
	{
		cameraComponent.orthographicSize = zoomInCameraSize;

		// ���ο� ����� �����մϴ�
		Time.timeScale = slowMotionFactor;

		yield return new WaitForSeconds(zoomInDelayTime * slowMotionFactor);

		Time.timeScale = 1f;

		// ī�޶� �÷��̾ �����ϴ� ���� ����մϴ�
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
	/// ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ �����·� �����ϴ�.
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
