using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;
using static UnityEngine.RuleTile.TilingRuleOutput;

[FSMState((int)EnemyController.EnemyState.MoveIdle)]
public class EnemyMoveIdleState : UnitState<EnemyController>
{
	private bool isForward;
	private Vector3 position;
	private float minX = 1.5f;
	private float maxX = 4f;
	private float minZ = 1.5f;
	private float maxZ = 4f;

	public override void Begin(EnemyController unit)
	{
		if (unit.moveIdleSpot == null)
		{
			unit.moveIdleSpot = GameObject.Instantiate(new GameObject("meleeIdlePos"),
				unit.transformParent == null ? null : unit.transformParent.transform);
		}

		unit.animator.SetBool("Move", true);

		if(isForward)
		{
			position =	
				new Vector3(unit.transform.position.x + Random.Range(minX, maxX),
				0f,
				unit.transform.position.z + Random.Range(minZ, maxZ));
			isForward = false;
		}
		else
		{
			position = 
				new Vector3(unit.transform.position.x - Random.Range(minX, maxX),
				0f,
				unit.transform.position.z - Random.Range(minZ, maxZ));
			isForward = true;
		}

		unit.moveIdleSpot.transform.position = position;

		FDebug.Log("EnemyMoveIdle");
	}

	public override void Update(EnemyController unit)
	{

		//unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(position), 23.0f * Time.deltaTime);
		unit.transform.LookAt(position);
		unit.transform.position = Vector3.MoveTowards(unit.transform.position,
			unit.moveIdleSpot.transform.position,
		unit.enemyData.Speed * Time.deltaTime);

		if (unit.transform.position == unit.moveIdleSpot.transform.position)
		{
			unit.rigid.velocity = Vector3.zero;
			if (!unit.IsCurrentState(EnemyState.Idle))
			{
				unit.ChangeState(EnemyState.Idle);
			}
		}

	}

	public override void FixedUpdate(EnemyController unit)
	{
	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool("Move", false);
		FDebug.Log("EnemyMoveIdleEnd");
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player") && !unit.isChasing)
		{
			FDebug.Log("MoveIdle Chasing");
			unit.target = other.GetComponent<UnitBase>();
			if (!unit.IsCurrentState(EnemyController.EnemyState.Chase))
			{
				unit.ChangeState(EnemyController.EnemyState.Chase);
			}
		}
	}
}
