using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePart : Part, IActive
{
	// Active Behaviour
	private PlayerController playerController;

	private void Awake()
	{
		if (PartItemData.PartType != PartTriggerType.ACTIVE)
		{
			FDebug.Log($"{PartItemData.PartType}이 맞지 않습니다.");
			Debug.Break();
		}
	}

	public void RunActive(PlayerController pc)
	{
		playerController = pc;

		playerController.activePartIsActive = true;
		// PC Get Variable true
	}

	public void StopActive()
	{
		// PC Get Variable false
		playerController.activePartIsActive = false;
	}
}
