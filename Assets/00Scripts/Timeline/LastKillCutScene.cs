using UnityEngine;

public class LastKillCutScene : CutSceneBase
{
	protected override void Init() { }

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
	}
}

