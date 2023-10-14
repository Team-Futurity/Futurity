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
			DamageInfo info = new DamageInfo(trapUnit, unit, 0);
			info.SetDamage(TrapData.TrapDamage);
			unit.Hit(info);
		}

		DestroyTrap();
	}

	private void DestroyTrap()
	{
		Destroy(this.gameObject);
	}
}