using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
	public EnemyController ec;

	private float mjfYpos;
	private Vector3 mjfTargetPos;
	private EffectActiveData floorData = new EffectActiveData();

	public void ActiveEffect(int activeIndex)
	{
		ActivationEffect(activeIndex, ec.currentEffectData);
	}

	public void ActivationEffect(int activeIndex, EffectActiveData data)
	{
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
		ec.currentEffectData.position = ec.target.transform.position + new Vector3(0, 1.0f, 0);
		ec.currentEffectData.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
		ec.currentEffectData.parent = null;

		ec.atkCollider.transform.position = ec.currentEffectData.position;
	}

	public void EliteRangedPositioning()
	{
		ec.currentEffectData.activationTime = EffectActivationTime.InstanceAttack;
		ec.currentEffectData.target = EffectTarget.Target;
		ec.currentEffectData.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}

	public void M_JFUp(float yPos)
	{
		mjfYpos = yPos;
		ec.transform.position = new Vector3(ec.transform.position.x, mjfYpos, ec.transform.position.z);
	}

	public void M_JFTargeting()
	{
		mjfTargetPos = ec.target.transform.position;
		ec.currentEffectData.position = mjfTargetPos + new Vector3(0f, 0.6f, 0f);

		floorData.activationTime = EffectActivationTime.AttackReady;
		floorData.target = EffectTarget.Ground;
		floorData.index = 0;
		floorData.position = mjfTargetPos + new Vector3(0, 1.0f, 0);
		floorData.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
		floorData.parent = null;

		ActivationEffect(0, floorData);
	}

	public void M_JFDown()
	{
		ec.transform.position = mjfTargetPos;
	}

	#endregion

	public void KnockBack()
	{
		if (ec.ThisEnemyType != EnemyType.TutorialDummy)
		{
			Vector3 direction = ec.transform.position - ec.target.transform.position;
			ec.enemyData.Knockback(direction.normalized, ec.knockbackPower);
		}
	}
}