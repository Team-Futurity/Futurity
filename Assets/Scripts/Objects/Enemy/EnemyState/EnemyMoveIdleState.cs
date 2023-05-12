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
	private float rand;
	private float minF = 1.5f;
	private float maxF = 2.5f;

	public override void Begin(EnemyController unit)
	{
		rand = Random.Range(minF, maxF);

		if (unit.moveIdleSpot == null)
		{
			unit.moveIdleSpot = GameObject.Instantiate(new GameObject("meleeIdlePos"),
				unit.transformParent == null ? null : unit.transformParent.transform);
		}

		unit.animator.SetBool(unit.moveAnimParam, true);

		if(isForward)
		{
			position =	
				new Vector3(unit.transform.position.x + rand,
				0f,
				unit.transform.position.z + rand);
			isForward = false;
		}
		else
		{
			position = 
				new Vector3(unit.transform.position.x - rand,
				0f,
				unit.transform.position.z - rand);
			isForward = true;
		}

		unit.moveIdleSpot.transform.position = position;
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
			unit.ChangeState(EnemyState.Idle);
		}

	}

	public override void FixedUpdate(EnemyController unit)
	{
	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player") && !unit.isChasing)
		{
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(EnemyController.EnemyState.Chase);
		}
	}
}
