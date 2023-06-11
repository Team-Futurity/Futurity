using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static PlayerController;

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
				var enemyController = other.gameObject.GetComponent<EnemyController>();

				unit.playerData.Attack(enemy, attackNode.attackST);
				HitEffectPooling(unit, enemy.transform);
				if(!enemyController.isTutorialDummy)
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

	public void HitEffectPooling(PlayerController unit, Transform target)
	{
		attackNode = unit.curNode;

		if (attackNode.effectPoolManager == null) { return; }

		Vector3 rot = target.rotation.eulerAngles;
		rot.y *= -1;
		effect = attackNode.hitEffectPoolManager.ActiveObject(target.position + attackNode.hitEffectOffset, Quaternion.Euler(rot));
		var particles = effect.GetComponent<ParticleController>();

		if(particles != null) 
		{
			particles.Initialize(attackNode.hitEffectPoolManager);
		}
	}
}
