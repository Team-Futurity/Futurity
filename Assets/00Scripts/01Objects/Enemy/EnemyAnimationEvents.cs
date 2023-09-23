using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
	private EnemyController ec;
	


	private void Start()
	{
		ec = GetComponent<EnemyController>();
	}

	public void ActiveEffect(int activeIndex)
	{
		EffectActiveData data = ec.currentEffectData;
		EffectKey key = ec.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, activeIndex);

		var particles = key.EffectObject.GetComponent<ParticleActiveController>();

		if (particles != null)
		{
			particles.Initialize(ec.effectController, key);
		}
	}

	public void MeleeAttack()
	{
		ec.atkCollider.enabled = true;
	}

	public void EliteRangedPositioning()
	{
		ec.atkCollider.transform.position = ec.target.transform.position;
		//ec.effects[1].effectTransform.position = ec.target.transform.position;
	}
}