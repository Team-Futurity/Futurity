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
		//Death event
		if (status.GetStatus(StatusName.CURRENT_HP) <= 0)
		{
			if (!ec.IsCurrentState(EnemyState.Death))
			{
				ec.ChangeState(EnemyState.Death);
			}
		}


		ec.ChangeState(EnemyController.EnemyState.Hitted);
		status.SetStatus(StatusName.CURRENT_HP, status.GetStatus(StatusName.CURRENT_HP) - damage);
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
