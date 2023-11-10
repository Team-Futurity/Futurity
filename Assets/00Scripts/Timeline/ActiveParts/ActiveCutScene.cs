using UnityEngine;

public class ActiveCutScene : CutSceneBase
{
	private Animator playerAnimator;
	private readonly int ACTIVE_ALPHA_KEY = Animator.StringToHash("AlphaTrigger");
	private readonly int ACTIVE_BETA_KEY = Animator.StringToHash("BetaTrigger");

	protected override void Init()
	{
		base.Init();
		playerAnimator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		Time.timeScale = 1.0f;
		chapterManager.SetActiveMainUI(true);
	}

	public void TimeStop()
	{
		if (cutSceneType == ECutSceneType.ACTIVE_ALPHA)
		{
			playerAnimator.SetTrigger(ACTIVE_ALPHA_KEY);
		}
		else
		{
			playerAnimator.SetTrigger(ACTIVE_BETA_KEY);
		}
		
		Time.timeScale = 0.0f;
	}
}
