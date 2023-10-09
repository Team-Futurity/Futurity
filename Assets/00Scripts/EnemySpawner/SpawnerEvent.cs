using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAreaType
{
	NONEEVENT = -1,
	Area1 = 0,
	Area3
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
			case EAreaType.Area1:
				manager.isEventEnable = true;
				Debug.Log($"조건 만족 : {manager.CurWaveSpawnCount} / {dialogConditions[(int)EAreaType.Area1]}");
				break;
			
			case EAreaType.Area3:
				manager.isEventEnable = true;
				Debug.Log($"조건 만족 : {manager.CurWaveSpawnCount} / {dialogConditions[(int)EAreaType.Area3]}");
				break;
			
			default:
				return;
		}
	}

	public void AreaEndEvent(SpawnerManager manager, EAreaType areaType)
	{
		if (manager.CurWaveSpawnCount > 0 || manager.SpawnerListCount > 0)
		{
			return;
		}

		switch (areaType)
		{
			case EAreaType.Area1:
				enableCollider.enabled = true;
				TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
				return;
			
			case EAreaType.Area3:
				TimelineManager.Instance.Chapter1_EnableCutScene(EChapter1CutScene.AREA3_LASTKILL);
				stageMoveManager.EnableExitCollider();
				return;
		}
		
		TimelineManager.Instance.EnablePublicCutScene(EPublicCutScene.LASTKILLCUTSCENE);
		stageMoveManager.EnableExitCollider();
	}

}
