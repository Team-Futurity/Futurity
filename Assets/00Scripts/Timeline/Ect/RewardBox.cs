using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
	[HideInInspector] public bool isEnable = false;
	private void OnTriggerStay(Collider other)
	{
		if (isEnable == false)
		{
			return;
		}
		
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				StageMoveManager.Instance.EnableExitCollider();
				gameObject.SetActive(false);
			}
		}
	}
}
