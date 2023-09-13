using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.BasicSM)]
public class PlayerBasicPartState : PlayerSpecialMoveState<BasicActivePart>
{
	private const float explosionEffectUnitSize = 0.1f;
	private const float maxAngle = 360;
	private readonly string IsActivePartAnimKey = "IsActivePart";
	private List<UnitBase> enemies = new List<UnitBase>();

	// effects
	private Transform chargeEffect;
	private Transform explosionEffect;

	private bool isExplosion;

	private float minSize;
	private float maxSize;

	private PlayerController pc;
	private Transform colliderOriginParent;
	private float initialYPosition;

	private float lastFrameTime;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		enemies.Clear();
		minSize = proccessor.minRange * MathPlus.cm2m;
		maxSize = proccessor.maxRange * MathPlus.cm2m;
		unit.animator.SetBool(IsActivePartAnimKey, true);
		unit.attackCollider.SetCollider(maxAngle, minSize);

		pc = unit;
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);
		
		if(currentTime == Time.deltaTime) { return; }

		if(isExplosion)
		{
			float radius = Mathf.Lerp(unit.attackCollider.radius, maxSize, proccessor.duration / Time.deltaTime);
			float effectRadius = 2 * radius * explosionEffectUnitSize;
			unit.attackCollider.SetCollider(maxAngle, radius);
			
			explosionEffect.localScale = new Vector3(effectRadius, effectRadius, effectRadius);
			pc.attackCollider.transform.position = explosionEffect.transform.position;

			if (currentTime >= proccessor.duration)
			{
				effectRadius = 2 * maxSize * explosionEffectUnitSize;

				unit.attackCollider.SetCollider(maxAngle, maxSize);
				explosionEffect.localScale = new Vector3(effectRadius, effectRadius, effectRadius);

				isExplosion = false;
				lastFrameTime = Time.deltaTime;
			}
			else if(currentTime >= proccessor.duration + lastFrameTime)
			{
				//EndExtension(unit);
			}
		}
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);
		unit.animator.SetBool(IsActivePartAnimKey, false);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);

		if(other.CompareTag(unit.EnemyTag))
		{
			var enemy = other.GetComponent<UnitBase>();

			if(enemy == null) { return; }

			enemies.Add(enemy);
			//unit.buffProvider.SetBuff(enemy, proccessor.buffCode, proccessor.duration - currentTime);
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);
	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{
		base.OnCollisionStay(unit, collision);
	}

	private void EndExtension(PlayerController unit)
	{
		unit.attackCollider.SetCollider(maxAngle, proccessor.maxRange * MathPlus.cm2m);

		foreach(var enemy in enemies)
		{
			enemy.Hit(unit.playerData, proccessor.damage);
		}
	}

	public void Charging()
	{
		chargeEffect = proccessor.chargeEffectObjectPool.ActiveObject(proccessor.chargeEffectPos.position, proccessor.chargeEffectPos.rotation);
	}

	public void PreAttack()
	{
		pc.attackCollider.radiusCollider.enabled = true;

		

		FDebug.Log("Pre : " + currentTime);
	}

	public void Attack()
	{
		FDebug.Log("Attack : " + currentTime);

		Vector3 vec = new Vector3(proccessor.explosionEffectPos.position.x, initialYPosition, proccessor.explosionEffectPos.position.z);

		explosionEffect = proccessor.explosionEffectObjectPool.ActiveObject(vec, Quaternion.identity);
		explosionEffect.GetComponent<ParticleController>().Initialize(proccessor.explosionEffectObjectPool);
		proccessor.chargeEffectObjectPool.DeactiveObject(chargeEffect);

		float diameter = 2 * minSize * explosionEffectUnitSize;
		explosionEffect.localScale = new Vector3(diameter, diameter, diameter);
		currentTime = 0;

		pc.attackCollider.transform.position = explosionEffect.transform.position;

		isExplosion = true;
	}

	public void Landing()
	{
		proccessor.landingEffectObjectPool.ActiveObject(proccessor.landingEffectPos.position, proccessor.landingEffectPos.rotation).
			GetComponent<ParticleController>().Initialize(proccessor.landingEffectObjectPool);
		pc.attackCollider.transform.localPosition = Vector3.zero;
		EndExtension(pc);
	}

	public void AttackEnd()
	{
		pc.attackCollider.transform.localPosition = Vector3.zero;
		pc.ChangeState(PlayerState.Idle);
	}
}
