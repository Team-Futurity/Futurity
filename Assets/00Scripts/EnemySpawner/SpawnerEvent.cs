using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAreaType
{
	NONEEVENT = -1,
	AREA1 = 0,
	AREA3,
	BOSS
}
public class SpawnerEvent : MonoBehaviour
{
	[Header("Component")] 
	[SerializeField] private StageMoveManager stageMoveManager;
	
	[Header("진행중 다이얼로그 출현 조건")] 
	[SerializeField] private List<int> dialogConditions;

	[Header("활성화 콜라이더")] 
	[SerializeField] private Collider enableCollider;
	
	public void InterimEvent(SpawnerManager manager, EAreaType areaType)
	{
		if (areaType == EAreaType.NONEEVENT || manager.isEventEnable == true || 
		    manager.CurWaveSpawnCount > dialogConditions[(int)areaType])
		{
			return;
		}
		
		// TODO : 각 조건에 맞는 다이얼로그 실행
		switch (areaType)
		{
			case EAreaType.AREA1:
				manager.isEventEnable = true;
				Debug.Log($"조건 만족 : {manager.CurWaveSpawnCount} / {dialogConditions[(int)EAreaType.AREA1]}");
				break;
			
			case EAreaType.AREA3:
				manager.isEventEnable = true;
				Debug.Log($"조건 만족 : {manager.CurWaveSpawnCount} / {dialogConditions[(int)EAreaType.AREA3]}");
				break;
			
			default:
				return;
		}
	}

	public void AreaEndEvent(SpawnerManager manager, EAreaType areaType)
	{
		if (manager.CurWaveSpawnCount > 0 || manager.SpawnerListCount > 0 || manager.AreaType == EAreaType.BOSS)
		{
			return;
		}

		switch (areaType)
		{
			case EAreaType.AREA1:
				enableCollider.enabled = true;
				TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
				return;
			
			case EAreaType.AREA3:
				TimelineManager.Instance.Chapter1_EnableCutScene(EChapter1CutScene.AREA3_LASTKILL);
				stageMoveManager.EnableExitCollider();
				return;
		}
		
		TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
		stageMoveManager.EnableExitCollider();
	}

}
