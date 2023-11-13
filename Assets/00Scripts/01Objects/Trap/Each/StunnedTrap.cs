using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedTrap : TrapBehaviour
{
	public override void ActiveTrap(List<UnitBase> units)
	{
		foreach (var unit in units) 
		{
	//		buffProvider.SetBuff(unit, 1002);
		}
	}
}
