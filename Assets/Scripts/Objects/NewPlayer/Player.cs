using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{

	[SerializeField] private PlayerController pc;


	private void Awake()
	{
		if(pc is null)
		{
			FDebug.Log($"{pc.GetType()}이 존재하지 않습니다.");
		}
	}

	private void Update()
	{
		if (pc.isComboState)
			pc.ComboTimer();
	}

	public override void Attack(UnitBase target)
	{
		target.Hit(this, GetDamage());
	}

	public override void Hit(UnitBase attacker, float damage)
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
		CurrentHp -= damage;
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
}
