using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDot : BuffBehaviour
{
	public override void Active()
	{
		base.Active();
		
		buffStay.AddListener(DotHit);
	}

	public override void UnActive()
	{
		buffStay.RemoveListener(DotHit);
		
		base.UnActive();
	}

	public void DotHit()
	{
		var damage = BuffData.BuffStatus.GetElement(StatusType.ATTACK_POINT).GetValue();
		targetUnit.status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);
	}
}
