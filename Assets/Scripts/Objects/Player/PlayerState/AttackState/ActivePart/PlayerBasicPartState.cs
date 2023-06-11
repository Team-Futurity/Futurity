using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.BasicPart)]
public class PlayerBasicPartState : PlayerActivePartAttackState<BasicActivePart>
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

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		enemies.Clear();
		isExplosion = false;
		minSize = proccessor.minRange * PlayerController.cm2m;
		maxSize = proccessor.maxRange * PlayerController.cm2m;
		unit.animator.SetBool(IsActivePartAnimKey, true);
		unit.attackCollider.SetCollider(maxAngle, minSize);

		pc = unit;
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);

		if(isExplosion)
		{
			float radius = Mathf.Lerp(unit.attackCollider.radius, maxSize, proccessor.duration / Time.deltaTime);
			float effectRadius = radius * explosionEffectUnitSize;
			unit.attackCollider.SetCollider(maxAngle, radius);
			
			explosionEffect.localScale = new Vector3(effectRadius, effectRadius, effectRadius);

			if (currentTime >= proccessor.duration)
			{
				effectRadius = maxSize * explosionEffectUnitSize;

				unit.attackCollider.SetCollider(maxAngle, maxSize);
				explosionEffect.localScale = new Vector3(effectRadius, effectRadius, effectRadius);

				isExplosion = false;
				EndExtension(unit);
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
			unit.buffProvider.SetBuff(enemy, proccessor.buffCode, proccessor.duration - currentTime);
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
		unit.attackCollider.SetCollider(maxAngle, proccessor.maxRange * PlayerController.cm2m);

		foreach(var enemy in enemies)
		{
			enemy.Hit(unit.playerData, proccessor.damage);
		}
	}

	public void Charging()
	{
		chargeEffect = proccessor.chargeEffectObjectPool.ActiveObject(proccessor.chargeEffectPos.position, proccessor.chargeEffectPos.rotation);
	}

	public void Attack()
	{
		pc.attackCollider.radiusCollider.enabled = true;
		explosionEffect = proccessor.explosionEffectObjectPool.ActiveObject(proccessor.explosionEffectPos.position, Quaternion.identity);
		explosionEffect.GetComponent<ParticleController>().Initialize(proccessor.explosionEffectObjectPool);
		proccessor.chargeEffectObjectPool.DeactiveObject(chargeEffect);

		float diameter = minSize;
		explosionEffect.localScale = new Vector3(diameter, diameter, diameter);

		Vector3 curPos = new Vector3(pc.transform.position.x, pc.transform.position.y, pc.transform.position.z);
		pc.attackCollider.transform.position = curPos;

		isExplosion = true;
	}

	public void Landing()
	{
		proccessor.landingEffectObjectPool.ActiveObject(proccessor.landingEffectPos.position, proccessor.landingEffectPos.rotation).
			GetComponent<ParticleController>().Initialize(proccessor.landingEffectObjectPool);
	}

	public void AttackEnd()
	{
		pc.attackCollider.transform.position = pc.transform.position;
		pc.ChangeState(PlayerState.Idle);
	}
}
