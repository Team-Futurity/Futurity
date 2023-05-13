using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrap : TrapBehaviour
{
	[SerializeField] private DownTrapFallObject fallObj;
	
	protected override void OnTrapStart()
	{
		fallObj.StartFall();
		fallObj.targetHitEvent.AddListener(SetDamage);
	}
	protected override void OnTrapEnd()
	{
	}

	protected override void OnTrapReset()
	{
		fallObj.targetHitEvent.RemoveListener(SetDamage);		
		fallObj.Reset();
	}

	private void SetDamage()
	{
		var objList = GetAroundObject();

		foreach (var obj in objList)
		{
			obj.GetComponent<UnitBase>().Hit(this, trapData.damage);
		}
	}
}
