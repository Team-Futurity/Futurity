using UnityEngine;

public class PlayerDeathCutScene : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private GameObject[] disableUI;
	[SerializeField] [Tooltip("플레이어 사망시 호출할 Window")] private GameObject deathOpenWindow;
	[SerializeField] [Tooltip("panel을 생성할 canvas")] private Canvas panelCanvas;
	
	public void EndPlayerDeathCutScene()
	{
		WindowManager.Instance.WindowOpen(deathOpenWindow, panelCanvas.transform, false, 
			Vector2.zero, Vector2.one);
		
		TimelineManager.Instance.ResetCameraValue();
	}
	
	private void OnEnable()
	{
		foreach (var ui in disableUI)
		{
			ui.gameObject.SetActive(false);		
		}
	}
}
