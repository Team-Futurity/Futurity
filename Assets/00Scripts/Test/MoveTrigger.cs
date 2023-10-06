using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private GameObject interactionUI;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && spawnerManager.CurWaveSpawnCount <= 0)
		{
			interactionUI.SetActive(true);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && spawnerManager.CurWaveSpawnCount <= 0)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				FadeManager.Instance.FadeIn();
				Invoke("MovePlayer", 1.3f);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && interactionUI.activeSelf == true)
		{
			interactionUI.SetActive(false);
		}
	}

	private void MovePlayer()
	{
		SteageMove.Instance.MoveStage(SteageMove.EStageType.STAGE_3);
		FadeManager.Instance.FadeOut();
		TimelineManager.Instance.EnableCutScene(ECutScene.AREA3_ENTRYCUTSCENE);
	}
}
