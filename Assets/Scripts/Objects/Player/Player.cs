using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : UnitBase
{
	private PlayerController pc;

	private void Start()
	{
		pc = GetComponent<PlayerController>();
	}

	public void Attack(UnitBase target, float AttackST)
	{
		float criticalConf = GetCritical();
		target.Hit(this, GetDamage(AttackST) * criticalConf);
	}

	public override void Attack(UnitBase target)
	{
		float criticalConf = GetCritical();
		target.Hit(this, GetDamage(1) * criticalConf);
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

		status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);

		if(!pc.hitCoolTimeIsEnd) { return; }

		if(!pc.IsAttackProcess(true) && !pc.IsCurrentState(PlayerState.Dash) && !pc.playerData.isStun)
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
		throw new System.NotImplementedException();
	}
}
