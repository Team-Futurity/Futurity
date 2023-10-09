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
		
		TimelineManager.Instance.Chapter1_EnableCutScene(EChapter1CutScene.AREA1_REWARDCUTSCENE);
		rewardBox.isEnable = true;
		gameObject.SetActive(false);
	}
}
