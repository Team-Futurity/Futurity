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

	public void SpecificEffectActive(int index)
	{
		if (EnemyEffectManager.Instance.copySpecific[ec.effects[index].indexNum] != null)
			EnemyEffectManager.Instance.SpecificEffectActive(ec.effects[index].indexNum);
	}

	public void HitEffectActive()
	{
		if (ec.isAttackSuccess && EnemyEffectManager.Instance.copyHit[ec.hitEffect.indexNum] != null)
		{
			ec.hitEffect.effectTransform.position = ec.target.transform.position + ec.hitEffect.effectExtraTransform;
			EnemyEffectManager.Instance.HitEffectActive(ec.hitEffect.indexNum);
			ec.isAttackSuccess = false;
		}
	}

	public void HittedEffectActive()
	{
		if (EnemyEffectManager.Instance.copyHitted[ec.hittedEffect.indexNum] != null)
		{
			ec.hittedEffect.effectTransform.position = ec.target.transform.position + ec.hitEffect.effectExtraTransform;
			EnemyEffectManager.Instance.HittedEffectActive(ec.hittedEffect.indexNum);
		}
	}

	public void MeleeAttack()
	{
		ec.atkCollider.enabled = true;
	}
}