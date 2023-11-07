
public class BossDeathCutScene : CutSceneBase
{
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		
	}

	public void BossDeath_StartSkeleton()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}
}
