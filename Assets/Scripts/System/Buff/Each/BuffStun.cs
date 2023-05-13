using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStun : BuffBehaviour
{
	// 아무런 행동도 할 수 없다.
	
	public override void Active(UnitBase unit)
	{
		base.Active(unit);
		
	}

	public override void UnActive()
	{
		
		base.UnActive();
	}
}
