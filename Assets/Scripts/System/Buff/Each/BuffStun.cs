using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStun : BuffBehaviour
{
	// �ƹ��� �ൿ�� �� �� ����.
	
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
