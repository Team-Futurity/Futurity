using UnityEngine;
using UnityEngine.Events;

public class EntryCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private GameObject playerCamera;

	[Header("진입 컷신에서 활성화할 오브젝트 목록")]
	[SerializeField] private GameObject[] walls;

	[Header("플레이어 이동값")] 
	[SerializeField] private GameObject targetPos;
	[SerializeField] private float duration;
	
	protected override void Init()
	{
		TimelineManager.Instance.uiCanvas.SetActive(false);
		TimelineManager.Instance.SetActivePlayerInput(false);
		Time.timeScale = 0.0f;
	}

	protected override void EnableCutScene() { }
	
	public override void DisableCutScene()
	{
		foreach (var wall in walls)
		{
			wall.SetActive(true);
		}
		TimelineManager.Instance.uiCanvas.SetActive(true);
		TimelineManager.Instance.SetActivePlayerInput(true);

		playerCamera.SetActive(true);
		gameObject.SetActive(false);
	}
	
	public void MovePlayer()
	{
		TimelineManager.Instance.PlayerController.LerpToWorldPosition(targetPos.transform.position, duration);
	}
}
