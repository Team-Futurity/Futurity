using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : UnitBase
{
	[SerializeField] private BossController bc;

	public override void Hit(DamageInfo damageInfo)
	{
		throw new System.NotImplementedException();
	}

	protected override void AttackProcess(DamageInfo damageInfo)
	{
		throw new System.NotImplementedException();
	}

	protected override float GetAttackPoint()
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDamage(float damageValue)
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}
