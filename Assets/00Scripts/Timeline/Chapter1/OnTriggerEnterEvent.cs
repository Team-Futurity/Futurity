using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
	// [SerializeField] private RewardBox rewardBox;
	// [SerializeField] private GameObject interactionUI;
	//
	// private void OnTriggerEnter(Collider other)
	// {
	// 	if (other.CompareTag("Player") == false && rewardBox.isCutScenePlayed == false)
	// 	{
	// 		return;
	// 	}
	//
	// 	if (rewardBox.isCutScenePlayed == false)
	// 	{
	// 		TimelineManager.Instance.EnableCutScene(ECutScene.AREA1_REWARDCUTSCENE);
	// 		rewardBox.isCutScenePlayed = true;
	// 	}
	// 	else if (rewardBox.isInteraction == true)
	// 	{
	// 		interactionUI.SetActive(true);
	// 	}
	// }
	//
	// private void OnTriggerStay(Collider other)
	// {
	// 	if (other.CompareTag("Player") && rewardBox.isInteraction == true)
	// 	{
	// 		if (Input.GetKeyDown(KeyCode.F))
	// 		{
	// 			TimelineManager.Instance.EnableCutScene(ECutScene.AREA1_EXITCUTSCENE);
	// 			interactionUI.SetActive(false);
	// 		}
	// 	}
	// }
	//
	// private void OnTriggerExit(Collider other)
	// {
	// 	if (other.CompareTag("Player") && interactionUI.activeSelf == true)
	// 	{
	// 		interactionUI.SetActive(false);
	// 	}
	// }
}
