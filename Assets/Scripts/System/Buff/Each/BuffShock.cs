using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffShock : BuffBehaviour
{
	
	private float currSpeed = .0f;
	private float buffSpeed = .0f;

	// ����� �̵��ӵ��� ���� �ð� ���� �����.
	// Move Speed�� ����

	public override void Active(UnitBase unit)
	{
		base.Active(unit);

	}

	public override void UnActive()
	{
		base.UnActive();
	}

}
