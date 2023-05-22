using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Hitted)]
public class EnemyHittedState : UnitState<EnemyController>
{
	private float curTime;
	private Material copyMat;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Hit Begin");
		curTime = 0;

		unit.rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

		unit.animator.SetTrigger(unit.hitAnimParam);
		if(copyMat == null )
			copyMat = new Material(unit.eMaterial);
		
		copyMat.SetColor("_MainColor", unit.damagedColor);

		unit.skinnedMeshRenderer.material = copyMat;

		//unit.rigid.AddForce(-unit.transform.forward * 450.0f, ForceMode.Impulse);
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

		unit.DelayChangeState(curTime, unit.hitMaxTime, unit, EnemyController.EnemyState.MDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{
		
	}

	public override void End(EnemyController unit)
	{
		unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.rigid.velocity = Vector3.zero;
		//FDebug.Log("Hit End");
		copyMat.SetColor("_MainColor", unit.defaultColor);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
