public class LastKillCutScene : CutSceneBase
{
	protected override void Init() { }

	protected override void EnableCutScene()
	{
		TimelineManager.Instance.ChangeFollowTarget(true);
	}

	public override void DisableCutScene()
	{
		TimelineManager.Instance.ChangeFollowTarget(false);
		gameObject.SetActive(false);
	}
}
