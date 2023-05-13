using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DownTrapFallObject : MonoBehaviour
{
	public UnityEvent targetHitEvent;
	
	public void StartFall()
	{
		gameObject.SetActive(true);
	}

	public void OnCollisionEnter(Collision coll)
	{
		if (coll.collider.CompareTag("Player"))
		{
			targetHitEvent?.Invoke();	
		}
	}
}
