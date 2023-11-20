using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)EnemyState.EliteRangedAttack)]
public class EliteRangedAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();
	private float radius = 1.6f;

	public EliteRangedAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Target;
		atk1.index = 0;
		atk1.position = new Vector3(-0.1f, -0.3f, -0.015f);
	}

	public override void Begin(EnemyController unit)
	{
		//unit.animator.SetTrigger(unit.ragnedAnimParam);
		
		atk1.parent = unit.test.gameObject;
		atk1.position = new Vector3(-0.1f, -0.3f, -0.015f);
		unit.currentEffectData = atk1;
		unit.atkCollider.radius = radius;
		unit.enemyData.status.GetStatus(StatusType.ATTACK_POINT).SetValue(55);
		unit.navMesh.enabled = true;
		unit.rigid.velocity = Vector3.zero;
		
	}


	public override void End(EnemyController unit)
	{
		unit.atkCollider.transform.position = unit.transform.position;
		base.End(unit);
	}
}
