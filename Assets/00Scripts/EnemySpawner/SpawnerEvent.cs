using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private ChapterMoveController chapterMove;
	[SerializeField] private UIDialogController dialogController;

	[Header("콜라이더 이벤트")] 
	[SerializeField] private Collider enableCollider;
	[SerializeField] private Collider disableCollider;
	
	public void InterimEvent(DialogData dialogData)
	{
		dialogController.gameObject.SetActive(true);
		dialogController.SetDialogData(dialogData);
		dialogController.PlayDialog();
	}

	public void SpawnerEndEvent(SpawnerManager manager, ESpawnerType spawnerType)
	{
		if (manager.CurWaveSpawnCount > 0 || manager.SpawnerListCount > 0 || manager.SpawnerType == ESpawnerType.CHAPTER_BOSS)
		{
			return;
		}

		switch (spawnerType)
		{
			case ESpawnerType.NONEVENT:
				break;
				
			case ESpawnerType.CHAPTER1_AREA1:
				CheckEndEventCollider();
				TimelineManager.Instance.EnableCutScene(ECutSceneType.LASTKILL);
				return;

			case ESpawnerType.CHAPTER1_AREA3:
				CheckEndEventCollider();
				TimelineManager.Instance.EnableCutScene(ECutSceneType.AREA3_EXIT);
				chapterMove.EnableExitCollider();
				return;
			
			case ESpawnerType.CHAPTER2_AREA1:
				break;
			
			case ESpawnerType.CHAPTER2_AREA2:
				break;
			
			case ESpawnerType.CHAPTER_BOSS:
				break;
			
			default:
				break;
		}
		
		TimelineManager.Instance.EnableCutScene(ECutSceneType.LASTKILL);
		chapterMove.EnableExitCollider();
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


