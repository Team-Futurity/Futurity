using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultAttack)]
public class MiniDefaultAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();

	private MiniDefaultAttackState()
	{
		atk1.activationTime = EffectActivationTime.MoveWhileAttack;
		atk1.target = EffectTarget.Caster;
		atk1.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack begin");
		base.Begin(unit);
		unit.animator.SetTrigger(unit.dashAnimParam);
		atk1.position = unit.transform.position;
		atk1.rotation = unit.transform.rotation;
		unit.atkCollider.enabled = true;
		unit.rigid.AddForce(unit.transform.forward * unit.powerReference1, ForceMode.Impulse);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack End");
		unit.atkCollider.enabled = false;
		base.End(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			//FDebug.Log("MiniDefault Attack Trigger");
			unit.enemyData.Attack(unit.target);
			unit.ChangeState(EnemyController.EnemyState.MiniDefaultKnockback);
		}
	}
}
