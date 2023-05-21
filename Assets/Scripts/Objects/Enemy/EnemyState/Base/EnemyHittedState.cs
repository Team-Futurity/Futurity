using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Hitted)]
public class EnemyHittedState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private Material copyMat;
	private Color defaultColor = Color.white;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Hit Begin");
		//unit.rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

		unit.animator.SetTrigger(unit.hitAnimParam);
		if(copyMat == null )
			copyMat = new Material(unit.eMaterial);
		unit.skinnedMeshRenderer.material = copyMat;
		copyMat.SetColor("_MainColor", unit.damagedColor);

		unit.rigid.AddForce(-unit.transform.forward * unit.hitPower, ForceMode.Impulse);
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

		unit.DelayChangeState(curTime, unit.hitMaxTime, unit, unit.UnitChaseState(unit));
	}

	public override void FixedUpdate(EnemyController unit)
	{
		
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Hit End");
		//unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.rigid.velocity = Vector3.zero;
		curTime = 0;
		copyMat.SetColor("_MainColor", defaultColor);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
