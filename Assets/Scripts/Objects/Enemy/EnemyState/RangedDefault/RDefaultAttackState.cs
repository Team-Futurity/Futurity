using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[FSMState((int)EnemyController.EnemyState.RDefaultAttack)]
public class RDefaultAttackState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private float attackTime = 0.6f;
	private bool isAttackDone = false;
	private float distance = .0f;
	private Vector3 projectilePos = new Vector3(0f, 0.75f, 0f);

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("RDefault attack Begin");
		unit.animator.SetTrigger(unit.atkAnimParam);
		unit.rangedProjectile.transform.position = unit.transform.position + projectilePos;
		/*unit.rangedProjectile.SetActive(true);*/
	}

	public override void Update(EnemyController unit)
	{
		distance = Vector3.Distance(unit.transform.position, unit.rangedProjectile.transform.position);
		curTime += Time.deltaTime;
		
		if(curTime > attackTime && !isAttackDone)
		{
			unit.rangedProjectile.SetActive(true);
			isAttackDone = true;
		}

		if (distance > unit.projectileDistance)
			unit.rangedProjectile.SetActive(false);

		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.RDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("RDefault attack End");
		unit.rangedProjectile.SetActive(false);
		curTime = 0f;
		isAttackDone = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			//FDebug.Log("RDefault attack Trigger");
			unit.enemyData.Attack(unit.target);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
