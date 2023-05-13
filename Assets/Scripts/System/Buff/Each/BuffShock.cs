using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	private float currSpeed = .0f;

	public override void Active(UnitBase unit)
	{
		base.Active(unit);

		currSpeed = targetUnit.status.GetStatus(StatusName.SPEED);
		var buffSpeed = BuffData.status.GetStatus(StatusName.SPEED);
		
		targetUnit.status.SetStatus(StatusName.SPEED, buffSpeed);

	}

	public override void UnActive()
	{
		targetUnit.status.SetStatus(StatusName.SPEED, currSpeed);

		base.UnActive();
	}

}
