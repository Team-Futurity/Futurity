using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
	private PlayerController pc;

	private void Start()
	{
		pc = GetComponent<PlayerController>();

		if (hpBar != null)
		{
			hpBar.SetGaugeFillAmount(status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue());
			FDebug.Log($"SetGaugeFillAmount : {status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue()}");
		}
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

		float remainingDamageRatio = Mathf.Clamp(1 - GetDefensePoint() * 0.01f, 0, 100);
		float finalDamage = damage * remainingDamageRatio;

		status.GetStatus(StatusType.CURRENT_HP).SubValue(finalDamage);
		pc.healthEffect.CheckPulseEffect(status.GetStatus(StatusType.CURRENT_HP).GetValue());
		hpBar.SetGaugeFillAmount(status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue());

		if(!pc.hitCoolTimeIsEnd) { return; }

		if(!pc.IsAttackProcess(true) && !pc.IsCurrentState(PlayerState.Dash) && !pc.playerData.isStun && !pc.IsCurrentState(PlayerState.BasicSM))
		{
			pc.ChangeState(PlayerState.Hit);
		}

		if (hpBar != null)
		{
			hpBar.SetGaugeFillAmount(status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue());
			FDebug.Log($"SetGaugeFillAmount : {status.GetStatus(StatusType.CURRENT_HP).GetValue() / status.GetStatus(StatusType.MAX_HP).GetValue()}");
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
