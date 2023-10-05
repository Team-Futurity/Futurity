using UnityEngine;
using UnityEngine.Events;

public class Area1_EntryCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private GameObject playerCamera;
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private DialogData dialogData;

	[Header("진입 컷신에서 활성화할 오브젝트 목록")]
	[SerializeField] private GameObject[] walls;

	[Header("플레이어 이동값")] 
	[SerializeField] private GameObject targetPos;
	[SerializeField] private float duration;
	
	protected override void Init()
	{
		TimelineManager.Instance.SetActiveMainUI(false);
		TimelineManager.Instance.SetActivePlayerInput(false);
	}

	protected override void EnableCutScene() { }
	
	public override void DisableCutScene()
	{
		foreach (var wall in walls)
		{
			wall.SetActive(true);
		}
		
		TimelineManager.Instance.SetActiveMainUI(true);
		TimelineManager.Instance.StartDialog(dialogData);

		playerCamera.SetActive(true);
	}
	
	public void MovePlayer()
	{
		TimelineManager.Instance.PlayerController.LerpToWorldPosition(targetPos.transform.position, duration);
	}

	public void Area1_SpawnEnemy()
	{
		spawnerManager.SpawnEnemy();
	}
}
