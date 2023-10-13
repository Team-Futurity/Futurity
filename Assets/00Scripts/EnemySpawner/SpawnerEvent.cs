using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private ChapterMoveController chapterMove;
	
	[Header("진행중 다이얼로그 출현 조건")] 
	[SerializeField] private List<int> dialogConditions;

	[Header("활성화 콜라이더")] 
	[SerializeField] private Collider enableCollider;
	
	public void InterimEvent(SpawnerManager manager, EChapterType chapterType)
	{
		if (chapterType == EChapterType.NONEVENTCHAPTER || manager.isEventEnable == true || 
		    manager.CurWaveSpawnCount > dialogConditions[(int)chapterType])
		{
			return;
		}
		
		// TODO : 각 조건에 맞는 다이얼로그 실행
		switch (chapterType)
		{
			case EChapterType.CHAPTER1_1:
				manager.isEventEnable = true;
				Debug.Log($"조건 만족 : {manager.CurWaveSpawnCount} / {dialogConditions[(int)EChapterType.CHAPTER1_1]}");
				break;
			
			case EChapterType.CHAPTER1_2:
				manager.isEventEnable = true;
				Debug.Log($"조건 만족 : {manager.CurWaveSpawnCount} / {dialogConditions[(int)EChapterType.CHAPTER1_2]}");
				break;
			
			default:
				return;
		}
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
				if (enableCollider != null)
				{
					enableCollider.enabled = true;	
				}
				TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
				return;
			
			case ESpawnerType.CHAPTER1_AREA2:
				break;
			
			case ESpawnerType.CHPATER1_AREA3:
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
