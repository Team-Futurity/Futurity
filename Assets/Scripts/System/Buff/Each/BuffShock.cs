using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	private float currSpeed = .0f;

	public override void Active(UnitBase unit)
	{
		base.Active(unit);

		var buffSpeed = BuffData.BuffStatus.GetStatusElement(StatusType.SPEED).GetValue();

		currSpeed = targetUnit.status.GetStatus(StatusType.SPEED).GetValue();
		targetUnit.status.GetStatus(StatusType.SPEED).MultipleValue(buffSpeed);

	}

	public override void UnActive()
	{
		targetUnit.status.GetStatus(StatusType.SPEED).SetValue(currSpeed);
		
		base.UnActive();
	}

}
