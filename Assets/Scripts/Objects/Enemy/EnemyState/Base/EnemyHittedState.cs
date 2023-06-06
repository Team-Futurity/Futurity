using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Hitted)]
public class EnemyHittedState : UnitState<EnemyController>
{
	private float curTime;
	private Color defaultColor = Color.white;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Hit Begin");
		curTime = 0;

		//unit.rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

		unit.animator.SetTrigger(unit.hitAnimParam);

		unit.skinnedMeshRenderer.material = unit.copyMat;

		unit.copyMat.SetColor(unit.matColorProperty, unit.damagedColor);
	}
	public override void Update(EnemyController unit)
	{
		//Death event
		if (unit.enemyData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
		{
			if (!unit.IsCurrentState(EnemyState.Death))
			{
				unit.ChangeState(EnemyState.Death);
			}
		}

		curTime += Time.deltaTime;

		unit.DelayChangeState(curTime, unit.hitMaxTime, unit, unit.UnitChaseState());
	}

	public override void FixedUpdate(EnemyController unit)
	{
		
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Hit End");

		//unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.rigid.velocity = Vector3.zero;
		unit.copyMat.SetColor(unit.matColorProperty, defaultColor);
		EnemyEffectManager.Instance.HittedEffectDeActive(unit.hittedEffect.indexNum);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
