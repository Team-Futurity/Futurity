using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StageEndPotalController : MonoBehaviour
{
	[SerializeField]
	public bool isActiveStageEndPortal = false;
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private GameObject barrierObject;
	[SerializeField]
	private GameObject cameraObject;
	[SerializeField]
	private WindowOpenController windowOpenController;

	[SerializeField]
	private float playerMoveLimit = 10f;
	private Vector3 playerStartPos;
	[SerializeField]
	private float playerSpeed = 1;

	[SerializeField]
	private float barrierSpeed = 3;
	[SerializeField]
	private float barrierEnd = 1;
	[SerializeField]
	private UnityEvent potalUIEvent;
	[SerializeField]
	private UnityEvent onBarrierReachedEndEvent;

	UnityEngine.InputSystem.PlayerInput playerInput;
	CameraController cameraController;
	PlayerController playerController;
	Animator animator;
	WindowManager windowManager;
	StageEndPotalManager chapterManager;
	GameObject currentPotalWindow;

	private Vector3 initialBarrierPosition;

	private Vector3 playerMoveDir;

	bool isMove = true;

	private void Start()
	{
		playerInput = player.gameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>();
		cameraController = cameraObject.GetComponent<CameraController>();
		playerController = player.GetComponent<PlayerController>();
		animator = player.GetComponent<Animator>();

		chapterManager = StageEndPotalManager.Instance;
		chapterManager.SetEndWarpPotal(this);

		isActiveStageEndPortal = false;
		windowManager = WindowManager.Instance;
		initialBarrierPosition = barrierObject.transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && isActiveStageEndPortal)
		{
			currentPotalWindow = windowOpenController.WindowActiveReturnOpen();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && isActiveStageEndPortal)
		{
			windowManager.WindowClose(currentPotalWindow);
		}
	}

	public void DialogStart()
	{
		potalUIEvent?.Invoke();
	}

	public void PotalStart()
	{
		cameraController.enabled = false;
		playerInput.enabled = false;
		playerMoveDir = barrierObject.transform.position - player.transform.position;
		playerMoveDir.y = 0;
		StartCoroutine(MoveBarrier());
	}

	IEnumerator MoveBarrier()
	{
		playerController.enabled = false;
		animator.SetBool("Move", false);
		bool isBarrierMove = true;
		while (isBarrierMove)
		{
			barrierObject.transform.position += Vector3.up * barrierSpeed * Time.deltaTime;

			if (Vector3.Distance(initialBarrierPosition, barrierObject.transform.position) >= barrierEnd)
			{
				isBarrierMove = false;
				StartCoroutine(MovePlayer());
			}

			yield return null;
		}
	}

	IEnumerator MovePlayer()
	{
		animator.SetBool("Move", true);
		Vector3 playerLookDir = player.transform.position - barrierObject.transform.position;
		playerStartPos = player.transform.position;
		StartCoroutine(BarrierEnd());

		while (isMove)
		{
			player.transform.position += playerMoveDir * playerSpeed * Time.deltaTime;

			Quaternion lookRotation = Quaternion.LookRotation(-playerLookDir);
			lookRotation.x = 0;
			lookRotation.z = 0;
			player.transform.rotation = lookRotation;

			if (Vector3.Distance(playerStartPos, player.transform.position) >= playerMoveLimit)
			{
				isMove = false;
				break;
			}

			yield return null;
		}
	}

	IEnumerator BarrierEnd()
	{
		yield return FadeManager.Instance.FadeCoroutineStart(false, 2, Color.black);

		chapterManager.ClearEndWarpPotal();
		isMove = false;
		cameraObject.GetComponent<CameraController>().enabled = true;
		player.gameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
		onBarrierReachedEndEvent.Invoke();
	}
}
