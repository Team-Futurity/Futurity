using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : TrapBehaviour
{
	private List<UnitBase> detectList;

	private LayerMask searchLayer = LayerMask.NameToLayer("Unit");
	public float explosionRadius = .0f;
	
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
		
		// DestroyTrap();
	}

	private void SearchAround()
	{
		var allUnit = Physics.OverlapSphere(transform.position, explosionRadius , searchLayer);

		foreach (var unit in allUnit)
		{
			detectList.Add(unit.GetComponent<UnitBase>());
		}
	}

	private void DestroyTrap()
	{
		Destroy(this.gameObject);
	}
}