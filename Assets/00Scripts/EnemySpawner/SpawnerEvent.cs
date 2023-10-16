using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private ChapterMoveController chapterMove;

	[Header("진행중 다이얼로그 출현 조건")] 
	[SerializeField] private GameObject dialogWindow;
	[SerializeField] private UIDialogController dialogController;
	[SerializeField] private List<int> dialogConditions;

	[Header("콜라이더 이벤트")] 
	[SerializeField] private Collider enableCollider;
	[SerializeField] private Collider disableCollider;
	
	public void InterimEvent(SpawnerManager manager, ESpawnerType chapterType)
	{
		if (chapterType == ESpawnerType.NONEVENT || manager.isEventEnable == true || 
		    manager.CurWaveSpawnCount > dialogConditions[(int)chapterType])
		{
			return;
		}

		if (manager.DialogData == null)
		{
			return;
		}
		
		manager.isEventEnable = true;
		dialogWindow.SetActive(true);
		dialogController.SetDialogData(manager.DialogData);
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
			case ESpawnerType.CHAPTER1_AREA1:
				if (enableCollider != null && disableCollider != null)
				{
					enableCollider.enabled = true;
					disableCollider.enabled = false;
				}
				TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
				return;
			
			case ESpawnerType.CHAPTER1_AREA2:
				break;
			
			case ESpawnerType.CHAPTER1_AREA3:
				TimelineManager.Instance.Chapter1_Area2_EnableCutScene(EChapter1_2.AREA2_LASTKILL);
				chapterMove.EnableExitCollider();
				break;
			
			case ESpawnerType.CHAPTER_BOSS:
				break;
			
			default:
				break;
		}
		
		TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
		chapterMove.EnableExitCollider();
	}
}
