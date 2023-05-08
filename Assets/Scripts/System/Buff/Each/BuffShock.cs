using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	private UnitBase targetUnit = null;
	
	private float currSpeed = .0f;
	private float buffSpeed = .0f;
	
	// ����� �̵��ӵ��� ���� �ð� ���� �����.
	// Move Speed�� ����

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
