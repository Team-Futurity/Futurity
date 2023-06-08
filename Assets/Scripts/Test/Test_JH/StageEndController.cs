using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StageEndController : MonoBehaviour
{
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private GameObject barrierObject;
	[SerializeField]
	private GameObject cameraObject;
	[SerializeField]
	private float playerSpeed = 1;
	[SerializeField]
	private float barrierSpeed = 3;
	[SerializeField]
	private float barrierEnd = 1;
	[SerializeField]
	private UnityEvent onBarrierReachedEnd;

	private Vector3 initialBarrierPosition;

	private Vector3 playerMoveDir;

	bool isMove = true;

	private void Start()
	{
		initialBarrierPosition = barrierObject.transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			cameraObject.GetComponent<CameraController>().enabled = false;
			playerMoveDir = barrierObject.transform.position - player.transform.position;
			playerMoveDir.y = 0;
			StartCoroutine(MoveBarrier());
		}
	}

	IEnumerator MoveBarrier()
	{
		player.GetComponent<PlayerController>().enabled = false;
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
		StartCoroutine(BarrierEnd());

		while (isMove)
		{
			player.transform.position += playerMoveDir * barrierSpeed * Time.deltaTime;

			yield return null;
		}
	}

	IEnumerator BarrierEnd()
	{
		yield return FadeManager.Instance.FadeCoroutineStart(false, 2, Color.black);
		isMove = false;
		cameraObject.GetComponent<CameraController>().enabled = true;
		onBarrierReachedEnd.Invoke();
	}
}
