using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdStatusDown : CrowdBase
{
	protected override void StartCrowd()
	{
		targetUnit.TryGetComponent<StatusManager>(out var status);
		status.SubStatus(data.BuffStatus.GetStatus());
	}

	protected override void ExitCrowd()
	{
		targetUnit.TryGetComponent<StatusManager>(out var status);
		status.AddStatus(data.BuffStatus.GetStatus());
	}
}
