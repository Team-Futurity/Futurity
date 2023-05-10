using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitBase
{
	public float attackDelay;

	private EnemyController ec;


	private void Start()
	{
		ec = GetComponent<EnemyController>();
	}

	public override void Attack(UnitBase target)
	{
		
	}

	public override void Hit(UnitBase attacker, float damage)
	{
		
	}

	protected override float GetAttakPoint()
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDamage()
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}
