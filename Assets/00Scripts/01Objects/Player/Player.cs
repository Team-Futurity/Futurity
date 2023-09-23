using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
	private PlayerController pc;

	private void Start()
	{
		pc = GetComponent<PlayerController>();
	}

	public override void Attack(UnitBase target, float AttackST = 1)
	{
		float criticalConf = GetCritical();
		target.Hit(this, GetDamage(AttackST) * criticalConf);
	}

	public override void Hit(UnitBase attacker, float damage, bool isDot = false)
	{
		//if (attacker.GetComponent<TestRangedEnemyAttackType>() != null)
		//{
		//	AudioManager.instance.PlayOneShot(pc.hitRanged, transform.position);
		//}
		//else
		//{ 
		//	AudioManager.instance.PlayOneShot(pc.hitMelee, transform.position);
		//}

		float remainingDamageRatio = Mathf.Clamp(1 - GetDefensePoint() * 0.01f, 0, 100);
		float finalDamage = damage * remainingDamageRatio;

		status.GetStatus(StatusType.CURRENT_HP).SubValue(finalDamage);

		if(!pc.hitCoolTimeIsEnd) { return; }

		if(!pc.IsAttackProcess(true) && !pc.IsCurrentState(PlayerState.Dash) && !pc.playerData.isStun && !pc.IsCurrentState(PlayerState.BasicSM))
		{
			pc.ChangeState(PlayerState.Hit);
		}
	}

	protected override float GetAttackPoint()
	{
		return status.GetStatus(StatusType.ATTACK_POINT).GetValue();
	}

	protected override float GetDamage(float attackST)
	{
		float atk = GetAttackPoint();
		return atk * attackST;
	}

	protected override float GetDefensePoint()
	{
		return status.GetStatus(StatusType.DEFENCE_POINT).GetValue();
	}
}
