using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using static EnemyController;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[FSMState((int)EnemyController.EnemyState.MoveIdle)]
public class EnemyMoveIdleState : UnitState<EnemyController>
{
	private Vector3 targetPos;
	private float randPercentage;
	private float randVec;
	private float distance;

	public override void Begin(EnemyController unit)
	{
		FDebug.Log("MoveIdle begin");

		unit.animator.SetBool(unit.moveAnimParam, true);

		randPercentage = Random.Range(0, 4);
		randVec = Random.Range(1.5f, 3.0f);

		if (randPercentage == 0)
		{
			targetPos = new Vector3(unit.transform.position.x + randVec, 0, unit.transform.position.z + randVec);
		}
		else if (randPercentage == 1)
		{
			targetPos = new Vector3(unit.transform.position.x - randVec, 0, unit.transform.position.z - randVec);
		}
		else if (randPercentage == 2)
		{
			targetPos = new Vector3(unit.transform.position.x - randVec, 0, unit.transform.position.z + randVec);
		}
		else if (randPercentage == 3)
		{
			targetPos = new Vector3(unit.transform.position.x + randVec, 0, unit.transform.position.z - randVec);
		}
	}

	public override void Update(EnemyController unit)
	{
		distance = Vector3.Distance(targetPos, unit.transform.position);

		if (distance < 1.0f)
			unit.ChangeState(EnemyController.EnemyState.Idle);

		unit.navMesh.SetDestination(targetPos);
	}

	public override void FixedUpdate(EnemyController unit)
	{
	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("Move Idle End");
		unit.rigid.velocity = Vector3.zero;
		unit.animator.SetBool(unit.moveAnimParam, false);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag) /*&& !unit.isChasing*/)
		{
			//FDebug.Log("Move Idle Trigger");
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(unit.UnitChaseState(unit));
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
