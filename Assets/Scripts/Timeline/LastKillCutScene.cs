using UnityEngine;

public class LastKillCutScene : CutSceneBase
{
	private PlayerController playerController;

	protected override void Init()
	{
		playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}

	protected override void EnableCutScene()
	{
		playerController.enabled = false;
	}

	public override void DisableCutScene()
	{
		playerController.enabled = true;
		gameObject.SetActive(false);
	}
}

