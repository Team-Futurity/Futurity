using UnityEngine;

public class LastKillCutScene : CutSceneBase
{
	[SerializeField] private BoxCollider disableCollider;
	[SerializeField] private BoxCollider enableCollider;
	
	protected override void Init() { }

	protected override void EnableCutScene()
	{
		TimelineManager.Instance.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		TimelineManager.Instance.SetActivePlayerInput(true);
		disableCollider.enabled = false;
		enableCollider.enabled = true;
		
		gameObject.SetActive(false);
	}
}

