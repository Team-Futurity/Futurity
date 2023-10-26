using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.Empty)]
public class StateBase : UnitState<EnemyController>
{
	protected float curTime = .0f;

	public override void Begin(EnemyController unit){}

	public override void End(EnemyController unit){}

	public override void FixedUpdate(EnemyController unit){}

	public override void OnCollisionEnter(EnemyController unit, Collision collision){}

	public override void OnTriggerEnter(EnemyController unit, Collider other){}

	public override void Update(EnemyController unit){}
}
