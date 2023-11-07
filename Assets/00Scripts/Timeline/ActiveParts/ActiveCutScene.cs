using UnityEngine;

public class ActiveCutScene : CutSceneBase
{
	private Animator playerAnimator;
	private readonly int ACTIVE_ALPHA_KEY = Animator.StringToHash("IsActivePart");

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
		playerAnimator.SetBool(ACTIVE_ALPHA_KEY, true);
		Time.timeScale = 0.0f;
	}
}
