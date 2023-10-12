using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEvents : MonoBehaviour
{
	public EnemyController ec;

	public bool isCharging = false;//
	
	private void Start()
	{
		
	}

	public void ActiveEffect(int activeIndex)
	{
		EffectActiveData data = ec.currentEffectData;
		EffectKey key = ec.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, activeIndex, false);

		var particles = key.EffectObject.GetComponent<ParticleActiveController>();

		if (particles != null)
		{
			particles.Initialize(ec.effectController, key);
		}
	}

	public void MeleeAttack()
	{
		ec.enemyData.EnableAttackTiming();
		ec.atkCollider.enabled = true;
	}
	public void RangedAttack()
	{
		ec.enemyData.EnableAttackTiming();
		ec.atkCollider.enabled = true;
	}
	public void EndAttack()
	{
		ec.atkCollider.enabled = false;
	}
	
	public void MeleeEffectPositioning()
	{
		ec.currentEffectData.position = new Vector3(-0.231f, 1.251f, 0.035f);
		ec.currentEffectData.rotation = Quaternion.Euler(new Vector3(-3.162f, 131.496f, 11.622f));
	}

	public void EliteRangedPositioning()
	{
		isCharging = false;
		ec.currentEffectData.parent = null;
		ec.atkCollider.transform.position = ec.target.transform.position;
		ec.currentEffectData.position = ec.target.transform.position;
		//ec.effects[1].effectTransform.position = ec.target.transform.position;
	}
}