using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStun : BuffBehaviour
{
	// 아무런 행동도 할 수 없다.
	
	public override void Active(UnitBase unit)
	{
		base.Active(unit);

		targetUnit.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

		if (targetUnit.transform.CompareTag("Player"))
		{
			
		}

	}

	public override void UnActive()
	{
		var rig = targetUnit.GetComponent<Rigidbody>();
		
		rig.constraints = RigidbodyConstraints.None;
		rig.constraints = RigidbodyConstraints.FreezeRotation;
		
		base.UnActive();
	}
}
