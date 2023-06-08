using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	private float currSpeed = .0f;

	public override void Active()
	{
		base.Active();

		var buffSpeed = BuffData.BuffStatus.GetElement(StatusType.SPEED).GetValue();

		currSpeed = targetUnit.status.GetStatus(StatusType.SPEED).GetValue();
		targetUnit.status.GetStatus(StatusType.SPEED).MultipleValue(buffSpeed);
	}

	public override void UnActive()
	{
		targetUnit.status.GetStatus(StatusType.SPEED).SetValue(currSpeed);
		base.UnActive();
	}
}
