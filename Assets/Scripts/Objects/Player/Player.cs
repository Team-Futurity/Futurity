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

	public override void Attack(UnitBase target)
	{
		target.Hit(this, GetDamage(1));
	}

	public override void Hit(UnitBase attacker, float damage, bool isDot)
	{
		//if (attacker.GetComponent<TestRangedEnemyAttackType>() != null)
		//{
		//	AudioManager.instance.PlayOneShot(pc.hitRanged, transform.position);
		//}
		//else
		//{ 
		//	AudioManager.instance.PlayOneShot(pc.hitMelee, transform.position);
		//}

		if(!pc.IsCurrentState(PlayerController.PlayerState.ChargedAttack))
		{
			pc.ChangeState(PlayerController.PlayerState.Hit);
		}
		
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);
	}

	protected override float GetAttackPoint()
	{
		return status.GetStatus(StatusType.ATTACK_POINT).GetValue();
	}

	protected override float GetDamage(float attackST)
	{
		return GetAttackPoint() * 1;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}
