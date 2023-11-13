using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdDot : CrowdBase
{
	protected override void StartCrowd()
	{
		StartCoroutine(UpdateDotDamage());
	}

	protected override void ExitCrowd()
	{
		StopCoroutine(UpdateDotDamage());
	}

	private IEnumerator UpdateDotDamage()
	{
		while (true)
		{
			targetUnit.Hit(new DamageInfo(
				null, null, data.BuffStatus.GetElement(StatusType.ATTACK_POINT).GetValue()
				));	
			yield return new WaitForSeconds(0.5f);
		}
	}
}
