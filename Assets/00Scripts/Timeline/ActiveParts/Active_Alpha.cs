using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Alpha : CutSceneBase
{
	private Animator playerAnimator;

	protected override void Init()
	{
		base.Init();
		playerAnimator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	public override void DisableCutScene()
	{
		Time.timeScale = 1.0f;
		chapterManager.SetActiveMainUI(true);
	}

	public void TimeStop()
	{
		playerAnimator.SetBool("IsActivePart", true);
		Time.timeScale = 0.0f;
	}
}
