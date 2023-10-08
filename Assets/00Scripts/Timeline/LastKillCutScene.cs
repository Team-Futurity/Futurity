using UnityEngine;

public class LastKillCutScene : CutSceneBase
{
	protected override void Init() { }

	protected override void EnableCutScene()
	{
		TimelineManager.Instance.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		TimelineManager.Instance.SetActivePlayerInput(true);
	}
}

