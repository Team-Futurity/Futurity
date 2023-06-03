using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MoveIdle)]
public class EnemyMoveIdleState : RandomMoveBaseState
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MoveIdle begin");
		unit.animator.SetBool(unit.moveAnimParam, true);

		base.Begin(unit);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		if (distance < 1.0f)
			unit.ChangeState(EnemyController.EnemyState.Idle);
	}

	public override void FixedUpdate(EnemyController unit)
	{
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Move Idle End");
		unit.animator.SetBool(unit.moveAnimParam, false);

		base.End(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag) /*&& !unit.isChasing*/)
		{
			//FDebug.Log("Move Idle Trigger");
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(unit.UnitChaseState(unit));

			if (unit.clusteringManager != null)
				unit.clusteringManager.AddEnemyInManager(unit);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
