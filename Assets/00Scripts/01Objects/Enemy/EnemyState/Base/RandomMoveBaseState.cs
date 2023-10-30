using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveBaseState : UnitState<EnemyController>
{
	private float randMoveDistanceMin = 1.0f;
	private float randMoveDistanceMax = 2.0f;

	private Vector3 targetPos;
	private float randPercentage;
	private float randDistance;
	protected float distance;

	public override void Begin(EnemyController unit)
	{
		randPercentage = Random.Range(0, 4);
		randDistance = Random.Range(randMoveDistanceMin, randMoveDistanceMax);

		if (randPercentage == 0)
			targetPos = new Vector3(unit.transform.position.x + randDistance, 0, unit.transform.position.z + randDistance);
		
		else if (randPercentage == 1)
			targetPos = new Vector3(unit.transform.position.x - randDistance, 0, unit.transform.position.z - randDistance);
		
		else if (randPercentage == 2)
			targetPos = new Vector3(unit.transform.position.x - randDistance, 0, unit.transform.position.z + randDistance);
		
		else if (randPercentage == 3)
			targetPos = new Vector3(unit.transform.position.x + randDistance, 0, unit.transform.position.z - randDistance);
		
	}

	public override void Update(EnemyController unit)
	{
		distance = Vector3.Distance(targetPos, unit.transform.position);

		unit.navMesh.SetDestination(targetPos);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.rigid.velocity = Vector3.zero;
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
