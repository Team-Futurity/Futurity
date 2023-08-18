using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// 이 클래스는 카메라를 지정된 위치로 이동시키는 역할을 합니다.
/// </summary>
public class CameraMover : MonoBehaviour
{
	[SerializeField]
	private Transform startTransform; // 카메라의 시작 위치를 결정하는 Transform
	private Vector3 startPostion;

	[SerializeField]
	private Transform endTransform; // 카메라의 종료 위치를 결정하는 Transform
	private Vector3 endPosiotion;

	[SerializeField]
	private GameObject moveCamera; // 이동할 카메라 객체
	private CameraController moveCameraController;

	[SerializeField]
	private float moveStartDelayTime = 1f;
	[SerializeField]
	private float moveEndDelayTime = 1f;
	WaitForSeconds moveDelayWaitForSeconds;

	[SerializeField]
	private float speed = 10f; // 카메라 이동 속도

	[SerializeField]
	private AnimationCurve easeGraph; // 이동에 적용할 easeGraph

	[SerializeField]
	private GameObject uiCanvas;

	[SerializeField]
	private GameObject player;
	private UnityEngine.InputSystem.PlayerInput playerInput;

	[SerializeField]
	private UnityEvent endMoveEvent;


	/// <summary>
	/// 시작 시 카메라를 이동시키는 코루틴을 시작합니다.
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
	/// 카메라를 시작 위치에서 종료 위치까지 이징 그래프에 따라 이동시킵니다.
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
	// 		float easedStep = easeGraph.Evaluate(fracJourney);  // 이징 그래프를 적용
	//
	// 		moveCamera.transform.position = Vector3.LerpUnclamped(startPostion, endPosiotion, easedStep);
	//
	// 		yield return null;  // 다음 프레임을 기다립니다
	// 	}
	//
	// 	yield return new WaitForSeconds(moveStartDelayTime);
	// 	Player statusPlayer = player.GetComponent<Player>();
	// 	uiCanvas.SetActive(true);
	// 	endMoveEvent.Invoke();
	// 	playerInput.enabled = true;
	// 	moveCameraController.enabled = true;  // 카메라 이동이 끝나면 CameraController를 활성화합니다.
	// 	statusPlayer.hpBar.GetComponent<GaugeBarController>().SetGaugeFillAmount(statusPlayer.status.GetStatus(StatusType.CURRENT_HP).GetValue() / statusPlayer.status.GetStatus(StatusType.MAX_HP).GetValue());
	// 	
	//
	// 	this.enabled = false;  // 이 스크립트는 비활성화합니다
	// }
}
