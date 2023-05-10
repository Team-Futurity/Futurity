using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
	[Space(10)]
	[Header("기타")]
	[Tooltip("대쉬 속도")]
	[SerializeField] private float dashSpeed = 15f;
	private PlayerController pc;

	public float DashSpeed => dashSpeed;

	private void Start()
	{
		pc = GetComponent<PlayerController>();
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

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}
}
