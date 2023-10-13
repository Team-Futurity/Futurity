using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitBase
{
	[SerializeField] private EnemyController ec;



	protected override void AttackProcess(DamageInfo damageInfo)
	{
		ec.isAttackSuccess = true;
		damageInfo.SetDamage(GetDamage(damageInfo.AttackST));
		damageInfo.Defender.Hit(damageInfo);
	}

	public override void Hit(DamageInfo damageInfo)
	{
		ec.ChangeState(EnemyController.EnemyState.Hitted);
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damageInfo.Damage);

		var hpElement = status.GetStatus(StatusType.CURRENT_HP).GetValue();
		var maxHpElement = status.GetStatus(StatusType.MAX_HP).GetValue();
		status.updateHPEvent?.Invoke(hpElement, maxHpElement);
	}

	protected override float GetAttackPoint()
	{
		return status.GetStatus(StatusType.ATTACK_POINT).GetValue();
	}

	protected override float GetDamage(float attackCount)
	{
		float value = GetAttackPoint() * attackCount *
		              (1 + Random.Range(-0.1f, 0.1f));
		return value;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}