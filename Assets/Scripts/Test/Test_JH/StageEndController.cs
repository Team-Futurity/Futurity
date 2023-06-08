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
	private float speed;
	[SerializeField]
	private float barrierEnd;
	[SerializeField]
	private UnityEvent onBarrierReachedEnd;

	private Vector3 initialBarrierPosition;
	private bool moving = false;

	private void Start()
	{
		initialBarrierPosition = barrierObject.transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			moving = true;
			StartCoroutine(MoveBarrierAndPlayer());
		}
	}

	IEnumerator MoveBarrierAndPlayer()
	{
		while (moving)
		{
			barrierObject.transform.position += Vector3.up * speed * Time.deltaTime;
			player.transform.position += Vector3.down * speed * Time.deltaTime;

			if (Vector3.Distance(initialBarrierPosition, barrierObject.transform.position) >= barrierEnd)
			{
				moving = false;
				onBarrierReachedEnd.Invoke();
			}

			yield return null;
		}
	}
}
