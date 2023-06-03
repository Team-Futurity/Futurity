using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : TrapBehaviour
{
	public override void ActiveTrap(List<UnitBase> units)
	{
		foreach(var unit in units)
		{
			unit.Hit(trapUnit, TrapData.TrapDamage);
		}

		DestroyTrap();
	}

	private void DestroyTrap()
	{
		Destroy(this.gameObject);
	}
}