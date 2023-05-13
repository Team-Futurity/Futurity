using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTrap : TrapBehaviour
{
	protected override void OnTrapStart()
	{
		var objectList = GetAroundObject();

		foreach (var obj in objectList)
		{
			if (obj.CompareTag("Player"))
			{
				obj.GetComponent<Rigidbody>().velocity += new Vector3(0f, 15f, 0f);
			}
			
			obj.GetComponent<UnitBase>().Hit(this,trapData.damage, true);
			// Buff 추가 예정
		}
	}
	protected override void OnTrapEnd()
	{
		
	}

	protected override void OnTrapReset()
	{
		
	}
	
}
