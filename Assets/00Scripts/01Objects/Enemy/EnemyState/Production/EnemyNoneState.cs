using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.None)]
public class EnemyNoneState : UnitState<EnemyController>
{

	public override void Begin(EnemyController unit) { }

	public override void End(EnemyController unit) { }

	public override void FixedUpdate(EnemyController unit) { }

	public override void OnCollisionEnter(EnemyController unit, Collision collision) { }

	public override void OnTriggerEnter(EnemyController unit, Collider other) { }

	public override void Update(EnemyController unit) { }
}
