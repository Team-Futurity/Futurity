using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Alpha : CutSceneBase
{
	private Animator playerAnimator;
	[SerializeField] private GameObject playerInfoUI;
	[SerializeField] private GameObject comboUI;

	protected override void Init()
	{
		base.Init();
		playerAnimator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		playerInfoUI.SetActive(false);
		comboUI.SetActive(false);
	}

	public override void DisableCutScene()
	{
		Time.timeScale = 1.0f;
		playerInfoUI.SetActive(true);
	}

	public void TimeStop()
	{
		playerAnimator.SetBool("IsActivePart", true);
		Time.timeScale = 0.0f;
	}
}
