using UnityEngine;
using UnityEngine.Events;

public class C2_A1_MainInteraction : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private Collider interactionCollider;
	[SerializeField] private UnityEvent disableEvent;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		base.EnableCutScene();
	}

	protected override void DisableCutScene()
	{
		interactionCollider.enabled = false;
		disableEvent?.Invoke();
	}

	public void C2A1_InteractionScripting()
	{
		StartScripting();
	}
}
