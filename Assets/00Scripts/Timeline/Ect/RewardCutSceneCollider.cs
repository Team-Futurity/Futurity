using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCutSceneCollider : MonoBehaviour
{
	[SerializeField] private RewardBox rewardBox;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		TimelineManager.Instance.EnableCutScene(ECutSceneType.CHAPTER1_AREA1_REWARD);
		gameObject.SetActive(false);
	}
}
