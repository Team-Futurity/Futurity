using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// �÷��̾��� ��� ��, ī�޶� �� �� �� ���ο� ��� ȿ���� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class DeathEffectController : MonoBehaviour
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
	private float normalCameraSize = 5f;
	[SerializeField]
	private float zoomInCameraSize = 2f;
	[SerializeField]
	private UnityEvent DeathEndEvent;

	private Camera cameraComponent;

	void Start()
	{
		// ī�޶� ������Ʈ�� �����ɴϴ�
		cameraComponent = cameraTransform.GetComponent<Camera>();
	}

	// �� �޼���� �÷��̾ ���Ϳ��� ������ Ÿ���� �Ծ��� �� ȣ��Ǿ�� �մϴ�
	public void PlayerDeath()
	{
		StartCoroutine(ZoomInAndSlowMotion());
	}

	IEnumerator ZoomInAndSlowMotion()
	{
		// ���ο� ����� �����մϴ�
		Time.timeScale = slowMotionFactor;

		// ī�޶� �÷��̾ �����ϴ� ���� ����մϴ�
		while (cameraComponent.orthographicSize > zoomInCameraSize)
		{
			cameraComponent.orthographicSize -= zoomInSpeed * Time.deltaTime;
			yield return null;
		}

		DeathEndEvent?.Invoke();
	}

	/// <summary>
	/// ��ũ��Ʈ�� ��Ȱ��ȭ�� �� ������ �����·� �����ϴ�.
	/// </summary>
	void OnDisable()
	{
		cameraComponent.orthographicSize = normalCameraSize;
		Time.timeScale = 1f;
	}
}
