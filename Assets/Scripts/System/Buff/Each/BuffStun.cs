using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStun : BuffBehaviour
{
	// 아무런 행동도 할 수 없다.
	
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
