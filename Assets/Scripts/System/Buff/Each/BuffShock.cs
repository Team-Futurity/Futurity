using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	private UnitBase targetUnit = null;
	
	private float currSpeed = .0f;
	private float buffSpeed = .0f;
	
	// 대상의 이동속도를 지속 시간 동안 낮춘다.
	// Move Speed의 변경

	private void Awake()
	{
		currBuffName = BuffName.SHOCK;
	}
	
	public override void Active(UnitBase unit, float activeTime)
	{
		targetUnit = unit;
		
		base.Active(targetUnit, activeTime);

		currSpeed = targetUnit.Speed;
		targetUnit.SetSpeed(buffSpeed);
	}

	public override void UnActive()
	{
		targetUnit.SetSpeed(currSpeed);
		
		base.UnActive();
	}

}
