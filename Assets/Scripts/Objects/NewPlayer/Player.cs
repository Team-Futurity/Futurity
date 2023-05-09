using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
	[Space(10)]
	[Header("��Ÿ")]
	[Tooltip("�뽬 �ӵ�")]
	[SerializeField] private float dashSpeed = 15f;
	private PlayerController pc;

	public float DashSpeed => dashSpeed;

	private void Start()
	{
		pc = GetComponent<PlayerController>();
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

		if (!isGodMode)
		{
			pc.ChangeState(PlayerController.PlayerState.Hit);
			CurrentHp -= damage;
		}
	}

	public override void DotHit(float damage)
	{
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
