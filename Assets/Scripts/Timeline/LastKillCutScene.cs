public class LastKillCutScene : CutSceneBase
{
	protected override void Init() { }

	protected override void EnableCutScene() { }

	public override void DisableCutScene()
	{
		gameObject.SetActive(false);
	}
}

