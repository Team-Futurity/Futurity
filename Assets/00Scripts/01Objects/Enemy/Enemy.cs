using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class Enemy : UnitBase
{
	[SerializeField] private EnemyController ec;



	protected override void AttackProcess(DamageInfo damageInfo)
	{
		damageInfo.SetDamage(GetDamage(damageInfo.AttackST));
		damageInfo.Defender.Hit(damageInfo);
	}

	public override void Hit(DamageInfo damageInfo)
	{
		/*FDebug.Log(ec.transform.position - ec.target.transform.position);
		FDebug.Log(ec.transform.eulerAngles.y);*/

		ec.knockbackPower = damageInfo.KnockbackPower;
		if (!ec.isDead)
		{
			PoolEffect(damageInfo);
			ec.ChangeState(EnemyState.Hitted);
		}
		status.GetStatus(StatusType.CURRENT_HP).SubValue(damageInfo.Damage);
		if (damageInfo.isCritical)
		{
			criticalImages.gameObject.SetActive(true);
			StartCoroutine("StartCriticalImage");
		}
		var hpElement = status.GetStatus(StatusType.CURRENT_HP).GetValue();
		var maxHpElement = status.GetStatus(StatusType.MAX_HP).GetValue();
		status.updateHPEvent?.Invoke(hpElement, maxHpElement);
	}
	protected override float GetAttackPoint()
	{
		return status.GetStatus(StatusType.ATTACK_POINT).GetValue();
	}
	protected override float GetDamage(float attackCount)
	{
		float value = GetAttackPoint() * attackCount *
		(1 + Random.Range(-0.1f, 0.1f));
		return value;
	}

	protected override float GetDefensePoint()
	{
		throw new System.NotImplementedException();
	}

	private void PoolEffect(DamageInfo damageInfo)
	{
		GameObject obj;
		Vector3 pos = ec.transform.position + new Vector3(0, 1.0f, 0);

		if (damageInfo.HitEffectPoolManager != null)
		{
			obj = damageInfo.HitEffectPoolManager.ActiveObject(pos).gameObject;

			InitializeEffect(obj, damageInfo.HitEffectPoolManager);
		}

		if (damageInfo.HitEffectPoolManagerByPart != null)
		{
			obj = damageInfo.HitEffectPoolManagerByPart.ActiveObject(pos).gameObject;

			InitializeEffect(obj, damageInfo.HitEffectPoolManagerByPart);
		}
	}

	private void InitializeEffect(GameObject obj, ObjectPoolManager<Transform> poolManager)
	{
		var particle = obj.GetComponent<ParticleController>();
		if (particle != null)
		{
			particle.Initialize(poolManager);
		}
	}
}