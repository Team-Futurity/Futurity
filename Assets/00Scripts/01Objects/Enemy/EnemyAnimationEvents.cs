using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEvents : MonoBehaviour
{
	public EnemyController ec;

	public void ActiveEffect(int activeIndex)
	{
		EffectActiveData data = ec.currentEffectData;
		EffectKey key = ec.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, activeIndex, false);
		if (ec.currentEffectKey == null)
			ec.currentEffectKey = new EffectKey();
		ec.currentEffectKey = key;

		var particles = key.EffectObject.GetComponent<ParticleActiveController>();

		if (particles != null)
		{
			particles.Initialize(ec.effectController, key);
		}
	}

	#region Attack
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
	public void EndRangedAttack()
	{
		ec.atkCollider.enabled = false;
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

	public void EliteFlooring()
	{
		ec.currentEffectData.activationTime = EffectActivationTime.AttackReady;
		ec.currentEffectData.target = EffectTarget.Ground;
		ec.currentEffectData.index = 0;
		ec.currentEffectData.position = ec.target.transform.position;
		ec.currentEffectData.parent = null;

		ec.atkCollider.transform.position = ec.currentEffectData.position;
	}

	public void EliteRangedPositioning()
	{
		ec.currentEffectData.activationTime = EffectActivationTime.InstanceAttack;
		ec.currentEffectData.target = EffectTarget.Target;
	}
	#endregion

	public void StopAnimationUntil(int frameCount)
	{
		ec.enemyData.StopAnimation(frameCount);
	}
}