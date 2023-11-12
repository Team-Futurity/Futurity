using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
	public readonly UnitBase Attacker;
	public readonly UnitBase Defender;
	public readonly float AttackST;
	public readonly float KnockbackPower;
	public float Damage { get; private set; }
	public ObjectPoolManager<Transform> HitEffectPoolManager { get; private set; }
	public Vector3 HitEffectOffset { get; private set; }
	public ObjectPoolManager<Transform> HitEffectPoolManagerByPart { get; private set; }
	public Vector3 HitEffectOffsetByPart { get; private set; }
	public Vector3 HitPoint { get; private set; }
	public bool IsDot { get; private set; }

	public bool isCritical = false;

	public DamageInfo(UnitBase attacker, UnitBase defender, float attackST, float knockbackPower = 0, bool isAttackCritical = false)
	{
		Attacker = attacker;
		Defender = defender;
		AttackST = attackST;
		KnockbackPower = knockbackPower;
		isCritical = isAttackCritical;
	}

	public DamageInfo(DamageInfo origin)
	{
		Attacker = origin.Attacker;
		Defender = origin.Defender;
		AttackST = origin.AttackST;
		KnockbackPower = origin.KnockbackPower;
		Damage = origin.Damage;
		HitEffectPoolManager = origin.HitEffectPoolManager;
		HitEffectOffset = origin.HitEffectOffset;
		HitEffectPoolManagerByPart = origin.HitEffectPoolManagerByPart;
		HitEffectOffsetByPart = origin.HitEffectOffsetByPart;
		HitPoint = origin.HitPoint;
		IsDot = origin.IsDot;
	}

	public void SetHitEffect(ObjectPoolManager<Transform> hitEffectPoolManager, Vector3? hitEffectOffset = null)
	{
		if(hitEffectPoolManager == null) { return; }

		HitEffectPoolManager = hitEffectPoolManager;
		HitEffectOffset = hitEffectOffset ?? Vector3.zero;
	}

	public void SetHitEffecByPart(ObjectPoolManager<Transform> hitEffectPoolManager, Vector3? hitEffectOffset = null)
	{
		if (hitEffectPoolManager == null) { return; }

		HitEffectPoolManagerByPart = hitEffectPoolManager;
		HitEffectOffsetByPart = hitEffectOffset ?? Vector3.zero;
	}

	public void SetDamage(float damage)
	{
		Damage = damage;
	}

	public void SetIsDot(bool isDot)
	{
		IsDot = isDot;
	}

	public void SetHitPoint(Vector3 hitPoint)
	{
		HitPoint = hitPoint;
	}
}