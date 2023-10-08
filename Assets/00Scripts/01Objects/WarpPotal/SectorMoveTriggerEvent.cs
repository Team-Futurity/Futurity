using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SectorMoveTriggerEvent : MonoBehaviour
{
	[Header("Trigger Info")] 
	public EStageType sectorCollider;
	[ReadOnly(false)] public UnityEvent enterEvent;
	[ReadOnly(false)] public UnityEvent stayEvent;
	[ReadOnly(false)] public UnityEvent exitEvent;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		enterEvent?.Invoke();
		StageMoveManager.Instance.SetActiveInteractionUI(true);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			if (sectorCollider == EStageType.CHAPTER1_AREA1)
			{
				TimelineManager.Instance.EnableCutScene(ECutScene.AREA1_EXITCUTSCENE);
				StageMoveManager.Instance.SetActiveInteractionUI(false);
				gameObject.SetActive(false);
				return;
			}
			
			if (sectorCollider == EStageType.CHAPTER1_AREA3)
			{
				StageMoveManager.Instance.MoveNextChapter();
				return;
			}
			
			StageMoveManager.Instance.SetActiveInteractionUI(false);
			StageMoveManager.Instance.MoveNextSector();
			gameObject.SetActive(false);
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		exitEvent?.Invoke();
		StageMoveManager.Instance.SetActiveInteractionUI(false);
	}
}
