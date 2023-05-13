using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	
	private float currSpeed = .0f;
	private float buffSpeed = .0f;

	// 대상의 이동속도를 지속 시간 동안 낮춘다.
	// Move Speed의 변경

	public override void Active(UnitBase unit)
	{
		base.Active(unit);

	}

	public override void UnActive()
	{
		base.UnActive();
	}

}
