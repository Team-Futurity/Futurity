using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[FSMState((int)EnemyController.EnemyState.MDefaultChase)]
public class MDefaultChaseState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase begin");
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.atkRange.enabled = true;
		unit.isChasing = true;
	}
	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), 15.0f * Time.deltaTime);
		//unit.transform.LookAt(unit.target.transform.position);
		unit.transform.position += unit.transform.forward.normalized * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase end");
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.atkRange.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			//FDebug.Log("MDefault Chase Trigger");
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
