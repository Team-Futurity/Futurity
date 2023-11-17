using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdStun : CrowdBase
{
	protected override void StartCrowd()
	{
		targetUnit.isStun = true;
	}

	protected override void ExitCrowd()
	{
		targetUnit.isStun = false;
	}
}
