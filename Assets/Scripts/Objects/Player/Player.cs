using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
	private PlayerController pc;
	private PartController partController;

	private void Start()
	{
		pc = GetComponent<PlayerController>();
		TryGetComponent(out partController);

		partController.SetOwnerUnit(this);
	}

	public override void Attack(UnitBase target)
	{
		OnAttack?.Invoke(target);
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

		if(!pc.IsCurrentState(PlayerController.PlayerState.ChargedAttack))
		{
			//pc.ChangeState(PlayerController.PlayerState.Hit);
		}
		
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damage);
	}

	protected override float GetAttakPoint()
	{
		return status.GetStatus(StatusType.ATTACK_POINT).GetValue();
	}

	protected override float GetDamage()
	{
		return GetAttakPoint();
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}
}
