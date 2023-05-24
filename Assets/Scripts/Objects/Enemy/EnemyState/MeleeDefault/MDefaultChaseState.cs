using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[FSMState((int)EnemyController.EnemyState.MDefaultChase)]
public class MDefaultChaseState : UnitState<EnemyController>
{
	private float distance = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase begin");
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;

		/*unit.isChasing = true;*/
	}
	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;

		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

		//unit.transform.rotation = Quaternion.LookRotation(unit.target.transform.position);
		unit.transform.LookAt(unit.target.transform.position);
		//unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), unit.turnSpeed * Time.deltaTime);
		if(distance < unit.attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
		}
		else if(distance > unit.attackRange)
			unit.transform.position += unit.transform.forward.normalized * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase end");
		unit.animator.SetBool(unit.moveAnimParam, false);

		/*unit.isChasing = false;*/
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
