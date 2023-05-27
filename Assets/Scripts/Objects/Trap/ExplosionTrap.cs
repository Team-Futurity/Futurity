using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : TrapBehaviour
{
	private List<UnitBase> detectList;

	private void Awake()
	{
		TryGetComponent(out trapUnit);
		detectList = new List<UnitBase>();
	}

	public override void ActiveTrap(List<UnitBase> units)
	{
		Attack();
	}

	private void Attack()
	{
		SearchAround();

		foreach (var unit in detectList)
		{
			unit.Hit(trapUnit, 0);
		}
		
	}

	private void SearchAround()
	{
		
	}

	private void DestroyTrap()
	{
		Destroy(this.gameObject);
	}
}