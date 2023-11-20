using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.MiniDefaultAttack)]
public class MiniDefaultAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();
	private float playerDistance = 0.5f;
	private float dashPower = 800f;
	private float knockbackPower = 380f;

	public MiniDefaultAttackState()
	{
		atk1.activationTime = EffectActivationTime.MoveWhileAttack;
		atk1.target = EffectTarget.Caster;
		atk1.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		unit.enemyData.EnableAttackTiming();
		unit.animator.SetTrigger(unit.dashAnimParam);
		atk1.position = unit.transform.position;
		atk1.rotation = unit.transform.rotation;
		unit.currentEffectData = atk1;
		unit.atkCollider.enabled = true;

		AudioManager.Instance.PlayOneShot(unit.attackSound1, unit.transform.position);
		unit.rigid.AddForce(unit.transform.forward * dashPower, ForceMode.Impulse);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackingDelay, unit, unit.UnitChaseState());
	}

	public override void End(EnemyController unit)
	{
		unit.atkCollider.enabled = false;
		unit.rigid.velocity = Vector3.zero;
		base.End(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			DamageInfo info = new DamageInfo(unit.enemyData, unit.target, 1);
			unit.enemyData.Attack(info);
			AudioManager.Instance.PlayOneShot(unit.attackSound2, unit.transform.position);
			KnockBack(unit);

			other.attachedRigidbody.velocity = Vector3.zero;
			unit.transform.position += (unit.transform.position - other.transform.position).normalized * playerDistance;
		}
	}

	private void KnockBack(EnemyController unit)
	{
		unit.currentEffectData.activationTime = EffectActivationTime.InstanceAttack;
		unit.currentEffectData.target = EffectTarget.Target;
		unit.currentEffectData.position = unit.target.transform.position + new Vector3(0, 1f, 0);

		unit.animator.SetTrigger(unit.atkAnimParam);
		AudioManager.Instance.PlayOneShot(unit.attackSound3, unit.transform.position);
		unit.rigid.AddForce(-unit.transform.forward * knockbackPower, ForceMode.Impulse);

	}
}
