using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
	private float moveStartDelayTime = 1f;
	[SerializeField]
	private float moveEndDelayTime = 1f;
	WaitForSeconds moveDelayWaitForSeconds;

	[SerializeField]
	private float speed = 10f; // ī�޶� �̵� �ӵ�

	[SerializeField]
	private AnimationCurve easeGraph; // �̵��� ������ easeGraph

	[SerializeField]
	private GameObject uiCanvas;

	[SerializeField]
	private GameObject player;
	private UnityEngine.InputSystem.PlayerInput playerInput;

	[SerializeField]
	private UnityEvent endMoveEvent;


	/// <summary>
	/// ���� �� ī�޶� �̵���Ű�� �ڷ�ƾ�� �����մϴ�.
	/// </summary>
	private void Start()
	{
		uiCanvas.SetActive(false);

		// if(moveCamera == null)
		// {
		// 	moveCamera = Camera.main.gameObject;
		// }
		// if(player == null)
		// {
		// 	player = GameObject.FindWithTag("Player");
		// }
		// playerInput = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
		// playerInput.enabled = false;
		//
		// moveDelayWaitForSeconds = new WaitForSeconds(moveStartDelayTime);
		// moveCameraController = moveCamera.GetComponent<CameraController>();
		// moveCameraController.enabled = false;
		//
		// startPostion = startTransform.position;
		// endPosiotion = endTransform.position + moveCameraController.offset;
		//
		// moveCamera.transform.position = startPostion;
		//
		// StartCoroutine(MoveCamera());
	}

	/// <summary>
	/// ī�޶� ���� ��ġ���� ���� ��ġ���� ��¡ �׷����� ���� �̵���ŵ�ϴ�.
	/// </summary>
	// private IEnumerator MoveCamera()
	// {
	// 	yield return moveDelayWaitForSeconds;
	//
	// 	float startTime = Time.time;
	// 	float journeyLength = Vector3.Distance(startPostion, endPosiotion);
	//
	// 	while (Vector3.Distance(moveCamera.transform.position, endPosiotion) > 0.01f)
	// 	{
	// 		float distCovered = (Time.time - startTime) * speed;
	// 		float fracJourney = distCovered / journeyLength;
	// 		float easedStep = easeGraph.Evaluate(fracJourney);  // ��¡ �׷����� ����
	//
	// 		moveCamera.transform.position = Vector3.LerpUnclamped(startPostion, endPosiotion, easedStep);
	//
	// 		yield return null;  // ���� �������� ��ٸ��ϴ�
	// 	}
	//
	// 	yield return new WaitForSeconds(moveStartDelayTime);
	// 	Player statusPlayer = player.GetComponent<Player>();
	// 	uiCanvas.SetActive(true);
	// 	endMoveEvent.Invoke();
	// 	playerInput.enabled = true;
	// 	moveCameraController.enabled = true;  // ī�޶� �̵��� ������ CameraController�� Ȱ��ȭ�մϴ�.
	// 	statusPlayer.hpBar.GetComponent<GaugeBarController>().SetGaugeFillAmount(statusPlayer.status.GetStatus(StatusType.CURRENT_HP).GetValue() / statusPlayer.status.GetStatus(StatusType.MAX_HP).GetValue());
	// 	
	//
	// 	this.enabled = false;  // �� ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�
	// }
}
