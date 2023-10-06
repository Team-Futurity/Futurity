using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
	[SerializeField] private RewardBox rewardBox;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false && rewardBox.isCutScenePlayed == false)
		{
			return;
		}

		if (rewardBox.isCutScenePlayed == false)
		{
			TimelineManager.Instance.EnableCutScene(ECutScene.AREA1_REWARDCUTSCENE);
			rewardBox.isCutScenePlayed = true;
		}
		else if (rewardBox.isInteraction == true)
		{
			TimelineManager.Instance.EnableCutScene(ECutScene.AREA1_EXITCUTSCENE);
		}
	}
}
