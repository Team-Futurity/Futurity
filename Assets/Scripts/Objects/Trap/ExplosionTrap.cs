using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : TrapBehaviour
{
	private List<UnitBase> detectList;

	private UnitBase trapUnit;
	// �ش� ����� ������ ���� ���, �ߵ��̹Ƿ� ���ǿ� ���ؼ� ��Ȯ�ؾ���.
	// Ž�� ���� ��ŭ Ž�� ��, ������ Ž���ϰ� �ش� ���鿡�� ������ �ο�

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
		// �ش� �޼��忡�� �ֺ� ���� Ž��
	}

	private void DestroyTrap()
	{
		Destroy(this.gameObject);
	}
}