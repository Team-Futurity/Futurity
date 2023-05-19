using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class Enemy : UnitBase
{ 
	[SerializeField] private EnemyController ec;


	private void Start()
	{

	}

	public override void Attack(UnitBase target)
	{
		target.Hit(this, GetDamage());
	}

	public override void Hit(UnitBase attacker, float damage, bool isDot)
	{
		ec.ChangeState(EnemyController.EnemyState.Hitted);
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);
	}

	protected override float GetAttakPoint()
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDamage()
	{
		return 10;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}
