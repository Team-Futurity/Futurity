using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerComboAttackState
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
		if(attackNode.slowTime > 0)
		{
			TimeScaleController.Instance.SetTimeScale(attackNode.slowScale, attackNode.slowTime, pc.transform.forward);
		}

		//pc.playerData.EnableAttackTiming();

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

		unit.attackColliderChanger.DisableAllCollider();

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
			if (unit.attackColliderChanger.GetCollider(attackNode.attackColliderType).IsInCuttedCollider(other.gameObject, attackNode.attackColliderType != ColliderType.Capsule))
			{
				var enemy = other.gameObject.GetComponent<UnitBase>();
				var enemyController = other.gameObject.GetComponent<EnemyController>();

				DamageInfo info = new DamageInfo(unit.playerData, enemy, attackNode.attackST);
				AttackAsset asset = attackNode.GetAttackAsset(0);
				info.SetHitEffect(asset.hitEffectPoolManager, asset.effectOffset);
				unit.playerData.Attack(info);
				//HitEffectPooling(unit, enemy.transform);
				if(!enemyController.isTutorialDummy)
				{
					Vector3 direction = enemy.transform.position - unit.transform.position;
					enemy.Knockback(direction.normalized, attackNode.attackKnockback);
				}
					
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
