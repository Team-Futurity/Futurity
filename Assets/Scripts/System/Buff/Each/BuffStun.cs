using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStun : BuffBehaviour
{
	public override void Active()
	{
		base.Active();

		targetUnit.isStun = true;

	}

	public override void UnActive()
	{
		targetUnit.isStun = false;
		
		base.UnActive();
	}
}
