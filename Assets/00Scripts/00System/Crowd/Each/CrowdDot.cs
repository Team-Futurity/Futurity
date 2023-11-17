using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdDot : CrowdBase
{
	private WaitForSeconds dotDelay;

	protected override void StartCrowd()
	{
		dotDelay = new WaitForSeconds(0.5f);
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
			var info = new DamageInfo(null, null, 0);
			info.SetDamage(data.BuffStatus.GetElement(StatusType.ATTACK_POINT).GetValue());
			targetUnit.Hit(info);
			yield return dotDelay;
		}
	}
}
