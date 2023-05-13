using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[FSMState((int)EnemyController.EnemyState.Chase)]
public class EnemyChaseState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool("Move", true);
		unit.chaseRange.enabled = false;
		unit.atkRange.enabled = true;
		unit.isChasing = true;
	}
	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		//unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), 30.0f * Time.deltaTime);
		unit.transform.LookAt(unit.target.transform.position);
		float distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
		unit.transform.position += unit.transform.forward * unit.enemyData.Speed * Time.deltaTime;
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool("Move", false);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player"))
		{
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyController.EnemyState.Attack);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
