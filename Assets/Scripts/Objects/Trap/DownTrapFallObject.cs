using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DownTrapFallObject : MonoBehaviour
{
	public UnityEvent targetHitEvent;
	public Rigidbody rig;
	
	private Vector3 originPos;
	
	public void StartFall()
	{
		originPos = transform.position;
		gameObject.SetActive(true);
	}

	public void Reset()
	{
		gameObject.SetActive(false);
		rig.velocity = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		
		transform.position = originPos;
	}

	public void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.CompareTag("Player"))
		{
			targetHitEvent?.Invoke();
		}
	}
}
