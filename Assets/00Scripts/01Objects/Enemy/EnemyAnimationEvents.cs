using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEvents : MonoBehaviour
{
	private EnemyController ec;

	private void Start()
	{
		ec = GetComponent<EnemyController>();
	}

	public void SpecificEffectActive(int index)
	{
		if (ec.effectManager.copySpecific[ec.effects[index].indexNum] != null)
			ec.effectManager.SpecificEffectActive(ec.effects[index].indexNum);
	}

	public void HitEffectActive()
	{
		if (ec.isAttackSuccess && ec.effectManager.copyHit[ec.hitEffect.indexNum] != null)
		{
			ec.hitEffect.effectTransform.position = ec.target.transform.position + ec.hitEffect.effectExtraTransform;
			ec.effectManager.HitEffectActive(ec.hitEffect.indexNum);
			ec.isAttackSuccess = false;
		}
	}

	public void HittedEffectActive()
	{
		if (ec.effectManager.copyHitted[ec.hittedEffect.indexNum] != null && !ec.isTutorialDummy)
		{
			ec.hittedEffect.effectTransform.position = ec.target.transform.position + ec.hitEffect.effectExtraTransform;
			ec.effectManager.HittedEffectActive(ec.hittedEffect.indexNum);
		}
	}

	public void MeleeAttack()
	{
		ec.atkCollider.enabled = true;
	}
	
	public void EliteRangedPositioning()
	{
		ec.atkCollider.transform.position = ec.target.transform.position;
		ec.effects[1].effectTransform.position = ec.target.transform.position;
	}
}