using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInColliderCheck : MonoBehaviour
{
	private EnemyController ec = null;
	private void Start()
	{
		ec = GetComponentInParent<EnemyController>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && ec.isInPlayer == false)
		{
			ec.isInPlayer = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && ec.isInPlayer == true)
		{
			ec.isInPlayer = false;
		}
	}
}
