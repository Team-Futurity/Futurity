using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : TrapBehaviour
{
	private List<UnitBase> detectList;

	private UnitBase trapUnit;
	// 해당 방식은 공격을 했을 경우, 발동이므로 조건에 대해서 명확해야함.
	// 탐색 범위 만큼 탐색 후, 적들을 탐색하고 해당 적들에게 데미지 부여

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
		// 해당 메서드에서 주변 유닛 탐색
	}

	private void DestroyTrap()
	{
		Destroy(this.gameObject);
	}
}