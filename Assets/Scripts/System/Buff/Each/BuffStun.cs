using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStun : BuffBehaviour
{
	// �ƹ��� �ൿ�� �� �� ����.
	
	public override void Active(UnitBase unit)
	{
		base.Active(unit);

		targetUnit.isStun = true;

	}

	public override void UnActive()
	{
		targetUnit.isStun = false;
		
		base.UnActive();
	}
}
