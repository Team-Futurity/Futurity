using UnityEngine;

public class PlayerDeathCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private GameObject[] disableUI;
	[SerializeField] [Tooltip("플레이어 사망시 호출할 Window")] private GameObject deathOpenWindow;
	[SerializeField] [Tooltip("panel을 생성할 canvas")] private Canvas panelCanvas;

	protected override void Init() { }
	
	protected override void EnableCutScene()
	{
		foreach (var ui in disableUI)
		{
			ui.gameObject.SetActive(false);		
		}
	}

	public override void DisableCutScene()
	{
		WindowManager.Instance.WindowOpen(deathOpenWindow, panelCanvas.transform, false, 
			Vector2.zero, Vector2.one);
		
		TimelineManager.Instance.ResetCameraValue();
	}
}
