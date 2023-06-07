using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDot : BuffBehaviour
{
	// 일정 시간 동안 Dot Damage를 받는다.

	public override void Active(UnitBase unit)
	{
		base.Active(unit);
		
		// Stay Event에 Hit를 넣어준다.
		buffStay.AddListener(DotHit);
	}

	public override void UnActive()
	{
		// Stay Event에서 Hit를 제거한다.
		buffStay.RemoveListener(DotHit);
		
		base.UnActive();
	}

	public void DotHit()
	{
		var damage = BuffData.BuffStatus.GetElement(StatusType.ATTACK_POINT).GetValue();
		targetUnit.status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);
	}
}
