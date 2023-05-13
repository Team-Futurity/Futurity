using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDot : BuffBehaviour
{
	// ���� �ð� ���� Dot Damage�� �޴´�.

	public override void Active(UnitBase unit)
	{
		base.Active(unit);
		
		// Stay Event�� Hit�� �־��ش�.
		buffStay.AddListener(DotHit);
	}

	public override void UnActive()
	{
		// Stay Event���� Hit�� �����Ѵ�.
		buffStay.RemoveListener(DotHit);
		
		base.UnActive();
	}

	public void DotHit()
	{
		targetUnit.Hit(null, BuffData.status.GetStatus(StatusName.ATTACK_POINT), true);
		
	}
}
