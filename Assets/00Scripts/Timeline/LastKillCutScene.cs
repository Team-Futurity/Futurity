using UnityEngine;

public class LastKillCutScene : CutSceneBase
{
	protected override void Init() { }

	protected override void EnableCutScene()
	{
		chapterManager.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		chapterManager.SetActivePlayerInput(true);
	}
}

