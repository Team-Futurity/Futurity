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

	public override void Attack(UnitBase target)
	{
		target.Hit(this, GetDamage());
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

		pc.ChangeState(PlayerController.PlayerState.Hit);
		status.SetStatus(StatusName.CURRENT_HP, status.GetStatus(StatusName.CURRENT_HP) - damage);
	}

	protected override float GetAttakPoint()
	{
		return 50;
	}

	protected override float GetDamage()
	{
		return GetAttakPoint();
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}

	public void SetSpeed(float speed)
	{
		status.SetStatus(StatusName.SPEED, speed);
	}
}
