using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.Death)]
public class EnemyDeathState : StateBase
{
	//private float deathDelay = 2.0f;
	private string copyDMatProperty = "_distortion";

	private EffectActiveData effectData = new EffectActiveData();
	public EnemyDeathState()
	{
		effectData.activationTime = EffectActivationTime.Hit;
		effectData.target = EffectTarget.Ground;
		effectData.index = 0;
		effectData.parent = null;
		
		effectData.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
	}
	public override void Begin(EnemyController unit)
	{
		effectData.position = unit.transform.position + new Vector3(0, 1.0f, 0);
		unit.effectController.ActiveEffect(effectData.activationTime, effectData.target, effectData.position, effectData.rotation, effectData.parent, effectData.index, 0, false);
		
		unit.animator.SetTrigger(unit.deadAnimParam);

		unit.isDead = true;
		unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.enemyCollider.enabled = false;

		unit.skinnedMeshRenderer.materials = new Material[1] { unit.copyDMat };
		unit.skinnedMeshRenderer.gameObject.layer = 7;
		
		unit.OnDisableEvent();
		unit.onDeathEvent?.Invoke();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		/*if (curTime > deathDelay)
		{
			if (unit.copyDMat.GetFloat(copyDMatProperty) > 0f)
				unit.copyDMat.SetFloat(copyDMatProperty, 1f - (curTime - deathDelay));
		}*/

		if (curTime > unit.deathDelay)
		{
			unit.gameObject.SetActive(false);
		}
	}
}
