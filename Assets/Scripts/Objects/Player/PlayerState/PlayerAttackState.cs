using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerAttackState : PlayerAttackBaseState
{
	// Animation Key
	protected readonly string AttackTriggerAnimKey = "AttackTrigger";
	protected readonly string AttackTypeAnimaKey = "ComboParam";

	// etc
	private Transform effect;
	//private CameraController cam;

	private int hittedEnemyCount;

	protected PlayerAttackState(string attackTriggerKey, string attackTypeKey)
	{
		AttackTriggerAnimKey = attackTriggerKey;
		AttackTypeAnimaKey = attackTypeKey;
	}

	public override void Begin(PlayerController pc)
	{
		base.Begin(pc);

		hittedEnemyCount = 0;

		pc.SetCollider(true);
	}

	public override void Update(PlayerController pc)
	{
		base.Update(pc);
		if(currentTime > attackNode.attackSpeed)
		{
			NextAttackState(pc, PlayerState.AttackAfterDelay);
		}
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		unit.rigid.velocity = Vector3.zero;

		unit.attackCollider.radiusCollider.enabled = false;

		bool isAttack = hittedEnemyCount > 0;
		unit.comboGaugeSystem.SetComboGaugeProc(isAttack, hittedEnemyCount);
		if(isAttack)
		{
			unit.hitCountSystem.AddHitCount(hittedEnemyCount);
		}
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if (other.CompareTag(unit.EnemyTag))
		{
			if (unit.attackCollider.IsInCollider(other.gameObject))
			{
				var enemy = other.gameObject.GetComponent<UnitBase>();
				unit.playerData.Attack(enemy);
				enemy.Knockback(unit.transform.forward, attackNode.attackKnockback);
				hittedEnemyCount++;
			}
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		
	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{

	}
}
