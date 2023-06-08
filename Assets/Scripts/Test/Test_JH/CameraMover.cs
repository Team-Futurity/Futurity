using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� Ŭ������ ī�޶� ������ ��ġ�� �̵���Ű�� ������ �մϴ�.
/// </summary>
public class CameraMover : MonoBehaviour
{
	[SerializeField]
	private Transform startTransform; // ī�޶��� ���� ��ġ�� �����ϴ� Transform
	private Vector3 startPostion;

	[SerializeField]
	private Transform endTransform; // ī�޶��� ���� ��ġ�� �����ϴ� Transform
	private Vector3 endPosiotion;

	[SerializeField]
	private GameObject moveCamera; // �̵��� ī�޶� ��ü
	private CameraController moveCameraController;

	[SerializeField]
	private float speed = 1.0f; // ī�޶� �̵� �ӵ�

	[SerializeField]
	private AnimationCurve easeGraph; // �̵��� ������ ��¡ �׷���

	/// <summary>
	/// ���� �� ī�޶� �̵���Ű�� �ڷ�ƾ�� �����մϴ�.
	/// </summary>
	private void Start()
	{
		moveCameraController = moveCamera.GetComponent<CameraController>();
		moveCameraController.enabled = false;

		startPostion = startTransform.position;
		endPosiotion = endTransform.position;

		StartCoroutine(MoveCamera());
	}

	/// <summary>
	/// ī�޶� ���� ��ġ���� ���� ��ġ���� ��¡ �׷����� ���� �̵���ŵ�ϴ�.
	/// </summary>
	private IEnumerator MoveCamera()
	{
		float startTime = Time.time;
		float journeyLength = Vector3.Distance(startPostion, endPosiotion);

		while (Vector3.Distance(moveCamera.transform.position, endPosiotion) > 0.01f)
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			float easedStep = easeGraph.Evaluate(fracJourney);  // ��¡ �׷����� ����

			moveCamera.transform.position = Vector3.LerpUnclamped(startPostion, endPosiotion, easedStep);

			yield return null;  // ���� �������� ��ٸ��ϴ�
		}

		moveCameraController.enabled = true;  // ī�޶� �̵��� ������ CameraController�� Ȱ��ȭ�մϴ�
		this.enabled = false;  // �� ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�
	}
}
