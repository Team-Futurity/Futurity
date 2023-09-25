using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : UnitBase
{
	[SerializeField] private EnemyController ec;



	protected override void AttackProcess(DamageInfo damageInfo)
	{
		ec.isAttackSuccess = true;
		damageInfo.Defender.Hit(this, GetDamage(1));
	}

	public override void Hit(UnitBase attacker, float damage, bool isDot = false)
	{
		ec.ChangeState(EnemyController.EnemyState.Hitted);
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);
	}

	protected override float GetAttackPoint()
	{
		throw new System.NotImplementedException();
	}

	protected override float GetDamage(float attackCount)
	{
		float value = status.GetStatus(StatusType.ATTACK_POINT).GetValue() * attackCount *
		              (1 + Random.Range(-0.1f, 0.1f));
		return value;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}