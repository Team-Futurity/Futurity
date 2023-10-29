using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private UIDialogController dialogController;
	[SerializeField] private GameObject chapterMoveTrigger;

	[Header("콜라이더 이벤트")] 
	[SerializeField] private Collider enableCollider;
	[SerializeField] private Collider disableCollider;

	private TimelineManager timeline;
	
	private void Start()
	{
		timeline = TimelineManager.Instance;
	}

	public void InterimEvent(DialogData dialogData)
	{
		dialogController.gameObject.SetActive(true);
		dialogController.SetDialogData(dialogData);
		dialogController.Play();
	}

	public void SpawnerEndEvent(SpawnerManager manager, ESpawnerType spawnerType)
	{
		if (manager.CurWaveSpawnCount > 0 || manager.SpawnerListCount > 0 || manager.SpawnerType == ESpawnerType.CHAPTER_BOSS)
		{
			return;
		}
		
		CheckEndEventCollider();
		chapterMoveTrigger.SetActive(true);
		timeline.EnableCutScene(spawnerType != ESpawnerType.CHAPTER1_AREA3 ? ECutSceneType.LASTKILL : ECutSceneType.CHAPTER1_AREA3_EXIT);
	}

	private void CheckEndEventCollider()
	{
		if (enableCollider != null)
		{
			enableCollider.enabled = true;
		}

		if (disableCollider != null)
		{
			disableCollider.enabled = false;
		}
	}
}


