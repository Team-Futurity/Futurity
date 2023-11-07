using UnityEngine;

public class C2_A1_MainInteraction : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private Collider interactionCollider;
	[SerializeField] private GameObject chapterTrigger;
	
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
		chapterTrigger.SetActive(true);
	}

	public void C2A1_InteractionScripting()
	{
		StartScripting();
	}
}
