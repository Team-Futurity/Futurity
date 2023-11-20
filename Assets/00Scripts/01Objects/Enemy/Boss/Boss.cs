using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : UnitBase
{
	[SerializeField] private BossController bc;



	public override void Hit(DamageInfo damageInfo)
	{
		//if (bc.curState == BossState.Chase || bc.curState == BossState.Idle)
		//	bc.ChangeState(BossState.Hit);
		//else
		if (!bc.isInPhase2Event)
		{
			bc.AddSubState(BossState.Hit);
			status.GetStatus(StatusType.CURRENT_HP).SubValue(damageInfo.Damage);
		}

		if (damageInfo.isCritical)
		{
			criticalImages.gameObject.SetActive(true);
			StartCoroutine("StartCriticalImage");
		}

		var hpElement = status.GetStatus(StatusType.CURRENT_HP).GetValue();
		var maxHpElement = status.GetStatus(StatusType.MAX_HP).GetValue();
		status.updateHPEvent?.Invoke(hpElement, maxHpElement);
	}

	protected override void AttackProcess(DamageInfo damageInfo)
	{
		damageInfo.SetDamage(GetDamage(damageInfo.AttackST));
		damageInfo.Defender.Hit(damageInfo);
	}

	protected override float GetAttackPoint()
	{
		return status.GetStatus(StatusType.ATTACK_POINT).GetValue();
	}

	protected override float GetDamage(float damageValue)
	{
		float value = (GetAttackPoint() + damageValue) / (1 + Random.Range(-0.1f, 0.1f));

		return value;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}
