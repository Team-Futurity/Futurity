using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StageEndController : MonoBehaviour
{
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private GameObject barrierObject;
	[SerializeField]
	private GameObject cameraObject;
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
	private UnityEvent onBarrierReachedEnd;

	UnityEngine.InputSystem.PlayerInput playerInput;
	CameraController cameraController;
	PlayerController playerController;
	Animator animator;

	private Vector3 initialBarrierPosition;

	private Vector3 playerMoveDir;

	bool isMove = true;

	private void Start()
	{
		playerInput = player.gameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>();
		cameraController = cameraObject.GetComponent<CameraController>();
		playerController = player.GetComponent<PlayerController>();
		animator = player.GetComponent<Animator>();
		initialBarrierPosition = barrierObject.transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			cameraController.enabled = false;
			playerInput.enabled = false;
			playerMoveDir = barrierObject.transform.position - player.transform.position;
			playerMoveDir.y = 0;
			StartCoroutine(MoveBarrier());
		}
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
		playerStartPos = player.transform.position;
		StartCoroutine(BarrierEnd());

		while (isMove)
		{
			player.transform.position += playerMoveDir * playerSpeed * Time.deltaTime;
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

		isMove = false;
		cameraObject.GetComponent<CameraController>().enabled = true;
		player.gameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
		onBarrierReachedEnd.Invoke();
	}
}
