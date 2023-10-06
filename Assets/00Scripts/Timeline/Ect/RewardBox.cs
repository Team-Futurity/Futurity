using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
	[HideInInspector] public bool isCutScenePlayed = false;
	[HideInInspector] public bool isInteraction = false;

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F) && isCutScenePlayed == true)
			{
				isInteraction = true;
				gameObject.SetActive(false);
			}
		}
	}
}
