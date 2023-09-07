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
	
	[Header("Event")] 
	[SerializeField] private UnityEvent entryCutSceneEndEvent;

	private GameObject player = null;
	private UnityEngine.InputSystem.PlayerInput playerInput;

	protected override void Init()
	{
		TimelineManager.Instance.uiCanvas.SetActive(false);
		
		if (player is null)
		{
			player = GameObject.FindWithTag("Player");
		}
		
		playerInput = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
		playerInput.enabled = false;
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
		playerInput.enabled = true;
		entryCutSceneEndEvent.Invoke();
		
		playerCamera.SetActive(true);
		gameObject.SetActive(false);
	}
	
	public void EnableEnemy()
	{
		Time.timeScale = 1.0f;
	}

	public void MovePlayer()
	{
		TimelineManager.Instance.PlayerController.LerpToWorldPosition(targetPos.transform.position, duration);
	}
}
