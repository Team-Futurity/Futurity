using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGod : CrowdBase
{
	protected override void StartCrowd()
	{
		targetUnit.isGodMode = true;
	}

	protected override void ExitCrowd()
	{
		targetUnit.isGodMode = false;
	}
}
