using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
	private PlayerController pc;

	protected override void Start()
	{
		base.Start();
		pc = GetComponent<PlayerController>();
	}

	protected override void AttackProcess(DamageInfo info)
	{
		//if(info.HitEffectPoolManager != null) { info.HitEffectPoolManager.ActiveObject(); }
		
		float criticalConf = GetCritical();
		info.SetDamage(GetDamage(info.AttackST) * criticalConf);
		info.isCritical = (criticalConf > 1.0f);

		info.Defender.Hit(info);
		onAttackEvent?.Invoke(info);
	}

	public override void Hit(DamageInfo damageInfo)
	{
		//if (attacker.GetComponent<TestRangedEnemyAttackType>() != null)
		//{
		//	AudioManager.instance.PlayOneShot(pc.hitRanged, transform.position);
		//}
		//else
		//{ 
		//	AudioManager.instance.PlayOneShot(pc.hitMelee, transform.position);
		//}

		if (pc.IsCurrentState(PlayerState.Death)) { return; }


		float remainingDamageRatio = Mathf.Clamp(1 - GetDefensePoint() * 0.01f, 0, 100);
		float finalDamage = damageInfo.Damage * remainingDamageRatio;

		var currentHP = status.GetStatus(StatusType.CURRENT_HP);
		currentHP.SubValue(finalDamage);

		var hpElement = currentHP.GetValue();
		var maxHpElement = status.GetStatus(StatusType.MAX_HP).GetValue();
		status.updateHPEvent?.Invoke(hpElement, maxHpElement);

		if(damageInfo.KnockbackPower > 0)
		{
			Knockback((damageInfo.Defender.transform.position - damageInfo.Attacker.transform.position).normalized, damageInfo.KnockbackPower);
		}

		Vector3 vec = damageInfo.Attacker.transform.position - damageInfo.Defender.transform.position;
		pc.transform.rotation = Quaternion.LookRotation(vec);

		if(currentHP.GetValue() <= 0)
		{
			pc.ChangeState(PlayerState.Death);

			return;
		}

		if(pc.IsCurrentState(PlayerState.Hit))
		{
			UnitState<PlayerController> state = null;
			pc.GetState(PlayerState.Hit, ref state);

			if(state != null)
			{
				var hitState = (PlayerHitState)state;
				hitState.HitProduction(pc);
			}
		}
		else if(/*!pc.IsAttackProcess(true) &&*/ !pc.IsCurrentState(PlayerState.Dash) && !pc.playerData.isStun && !pc.IsCurrentState(PlayerState.BasicSM))
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
