using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		Vector3 pos = damageInfo.HitPoint;

		if (damageInfo.HitEffectPoolManager != null)
		{
			Quaternion rotation = Quaternion.identity;
			Vector3 position = Vector3.zero;


			Quaternion enemyRoatation = Quaternion.LookRotation(damageInfo.Attacker.transform.forward);
			if (enemyRoatation.y > 180f) { enemyRoatation.y -= 360f; }
			rotation = enemyRoatation;
			//FDebug.Log($"Player Rotation : {playerRot.eulerAngles}\nEffect Rotation Offset : {attackNode.effectRotOffset.eulerAngles}\nResult : {rotation.eulerAngles}");

			position = pos + rotation * damageInfo.HitEffectOffset;
			position.y = pos.y + damageInfo.HitEffectOffset.y;
			obj = damageInfo.HitEffectPoolManager.ActiveObject(position, rotation).gameObject;

			//obj = damageInfo.HitEffectPoolManager.ActiveObject(pos + damageInfo.HitEffectOffset, ec.target.transform.rotation).gameObject;

			InitializeEffect(obj, damageInfo.HitEffectPoolManager);
		}

		if (damageInfo.HitEffectPoolManagerByPart != null)
		{
			Quaternion rotation = Quaternion.identity;
			Vector3 position = Vector3.zero;

			Quaternion enemyRoatation = Quaternion.LookRotation(damageInfo.Defender.transform.forward);
			if (enemyRoatation.y > 180f) { enemyRoatation.y -= 360f; }
			rotation = enemyRoatation;
			//FDebug.Log($"Player Rotation : {playerRot.eulerAngles}\nEffect Rotation Offset : {attackNode.effectRotOffset.eulerAngles}\nResult : {rotation.eulerAngles}");

			position = pos + rotation * damageInfo.HitEffectOffsetByPart;
			position.y = pos.y + damageInfo.HitEffectOffsetByPart.y;
			obj = damageInfo.HitEffectPoolManagerByPart.ActiveObject(position, rotation).gameObject;

			//obj = damageInfo.HitEffectPoolManagerByPart.ActiveObject(pos + damageInfo.HitEffectOffsetByPart).gameObject;

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