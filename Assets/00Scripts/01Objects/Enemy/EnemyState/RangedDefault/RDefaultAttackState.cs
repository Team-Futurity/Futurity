using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[FSMState((int)EnemyController.EnemyState.RDefaultAttack)]
public class RDefaultAttackState : EnemyAttackBaseState
{
	private float attackTime = 0.6f;
	private bool isAttackDone = false;
	private float distance = .0f;
	private Vector3 projectilePos = new Vector3(0f, 1.05f, 0f);

	private EffectActiveData effectActiveData = new EffectActiveData();

	public RDefaultAttackState()
	{
		effectActiveData.activationTime = EffectActivationTime.AttackReady;
		effectActiveData.target = EffectTarget.Caster;
		effectActiveData.position = new Vector3(0f, 1.1f, 0.7f);
		
		effectActiveData.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("RDefault attack Begin");
		base.Begin(unit);
		effectActiveData.parent = unit.gameObject;
		unit.currentEffectData = effectActiveData;
		unit.rangedProjectile.transform.position = unit.transform.position + projectilePos;
		unit.rangedProjectile.transform.rotation = unit.transform.rotation;
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);
		distance = Vector3.Distance(unit.transform.position, unit.rangedProjectile.transform.position);
		
		if(curTime > attackTime && !isAttackDone)
		{
			unit.rangedProjectile.SetActive(true);
			unit.enemyData.EnableAttackTiming();
			isAttackDone = true;
		}

		if (distance > unit.projectileDistance)
			unit.rangedProjectile.SetActive(false);
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("RDefault attack End");
		base.End(unit);
		isAttackDone = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		//FDebug.Log("RDefault attack Trigger");
		base.OnTriggerEnter(unit, other);
	}
}
