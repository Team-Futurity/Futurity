using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.Death)]
public class EnemyDeathState : StateBase
{
	private float deathDelay = 2.0f;
	private string copyDMatProperty = "_distortion";

	public override void Begin(EnemyController unit)
	{
		unit.isDead = true;
		unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.animator.SetTrigger(unit.deadAnimParam);
		unit.enemyCollider.enabled = false;

		unit.skinnedMeshRenderer.materials = new Material[1] { unit.copyDMat };
		unit.skinnedMeshRenderer.gameObject.layer = 7;
		
		unit.OnDisableEvent();
		unit.onDeathEvent?.Invoke();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		if (unit.copyDMat.GetFloat(copyDMatProperty) > 0f)
			unit.copyDMat.SetFloat(copyDMatProperty, 1f - curTime);

		if (curTime > deathDelay)
		{
			unit.gameObject.SetActive(false);
		}
	}
}
