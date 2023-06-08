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
		// ī�޶� ������Ʈ�� �����ɴϴ�
		cameraComponent = cameraTransform.GetComponent<Camera>();
	}

	// �� �޼���� �÷��̾ ���Ϳ��� ������ Ÿ���� �Ծ��� �� ȣ��Ǿ�� �մϴ�
	public void LastKill()
	{
		StartCoroutine(ReleaseTimer());
		motionCoroutine = StartCoroutine(SlowAndZoomInMotion());
	}

	private IEnumerator SlowAndZoomInMotion()
	{
		// ���ο� ����� �����մϴ�
		Time.timeScale = slowMotionFactor;

		// ī�޶� �÷��̾ �����ϴ� ���� ����մϴ�
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
	/// ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ �����·� �����ϴ�.
	/// </summary>
	private void OnDisable()
	{
		cameraComponent.orthographicSize = normalCameraSize;
		Time.timeScale = 1f;
	}
}
