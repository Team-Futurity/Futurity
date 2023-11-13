using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdHeal : CrowdBase
{
	protected override void StartCrowd()
	{
		targetUnit.onAttackEvent.AddListener(AddHealPoint);
	}

	protected override void ExitCrowd()
	{
		targetUnit.onAttackEvent.RemoveListener(AddHealPoint);
	}

	private void AddHealPoint(DamageInfo info)
	{
		targetUnit.TryGetComponent<StatusManager>(out var status);
		status.AddStatus(data.BuffStatus.GetStatus());
	}
}
