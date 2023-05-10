using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Hitted)]
public class EnemyHittedState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.hitAnimParam);
		unit.eMaterial.color = Color.red;
	}

	public override void Update(EnemyController unit)
	{
		unit.hitCurTime += Time.deltaTime;



		if (unit.hitCurTime > unit.hitMaxTime)
		{
			if (!unit.IsCurrentState(EnemyController.EnemyState.Chase))
			{
				unit.ChangeState(EnemyController.EnemyState.Chase);
			}
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{
		
	}

	public override void End(EnemyController unit)
	{
		unit.hitCurTime = 0;
		unit.eMaterial.color = unit.defaultColor;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		
	}

}
