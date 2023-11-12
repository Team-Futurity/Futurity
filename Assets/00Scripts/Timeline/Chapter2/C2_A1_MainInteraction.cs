using UnityEngine;
using UnityEngine.Events;

public class C2_A1_MainInteraction : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private UnityEvent disableEvent;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		base.EnableCutScene();
		
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		disableEvent?.Invoke();
		
		chapterManager.SetActiveMainUI(true);
	}

	public void C2A1_InteractionScripting()
	{
		StartScripting();
	}
}
