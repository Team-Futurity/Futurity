using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.BasicSM)]
public class PlayerBasicPartState : PlayerSpecialMoveState<BasicActivePart>
{
	private const float explosionEffectUnitSize = 0.1f;
	private const float maxAngle = 360;
	private List<UnitBase> enemies = new List<UnitBase>();

	// effects
	private Transform chargeEffect;
	private Transform explosionEffect;

	private bool isExplosion;

	private float minSize;
	private float maxSize;

	private PlayerController pc;
	private Transform colliderOriginParent;
	private float initialYPosition = 0.001f;

	private float lastFrameTime;

	// collider
	private TruncatedCapsuleCollider currentCollider;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		enemies.Clear();
		
		// Size 조정
		minSize = proccessor.minRange * MathPlus.cm2m;
		maxSize = proccessor.maxRange * MathPlus.cm2m;
		TimelineManager.Instance.EnableCutScene(ECutSceneType.ACTIVE_ALPHA);

		pc = unit;
		
		if(unit.attackColliderChanger.GetCollider(ColliderType.Capsule) is TruncatedCapsuleCollider capsuleCollider)
		{
			currentCollider = capsuleCollider;
			currentCollider.SetCollider(maxAngle, minSize);
		}
		else
		{
			FDebug.LogWarning("Collider could not Type Conversion.", GetType());
			return;
		}
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);
		
		if(currentTime == Time.deltaTime) { return; }

		if(isExplosion)
		{
			// 점점 원형이 커지게 설정하기 위해서 사용됨
			float radius = Mathf.Lerp(currentCollider.Length, maxSize, proccessor.duration / Time.deltaTime);
			float effectRadius = 2 * radius * explosionEffectUnitSize;
			currentCollider.SetCollider(maxAngle, radius);
			
			explosionEffect.localScale = new Vector3(effectRadius, effectRadius, effectRadius);
			currentCollider.transform.position = explosionEffect.transform.position;

			// 실행 시간이 지났다면
			if (currentTime >= proccessor.duration)
			{
				effectRadius = 2 * maxSize * explosionEffectUnitSize;

				// Max Size로 Collider를 설정한다.
				currentCollider.SetCollider(maxAngle, maxSize);
				// Effect 위치는 반경만큼
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
		unit.rigid.velocity = Vector3.zero;
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

	public void EndExtension()
	{
		currentCollider.SetCollider(maxAngle, proccessor.maxRange * MathPlus.cm2m);

		foreach(var enemy in enemies)
		{
			DamageInfo info = new DamageInfo(pc.playerData, enemy, 1);
			info.SetDamage(proccessor.damage);

			var effect = pc.hitEffectDatabase.GetHitEffect(404);

			if(effect != null)
			{
				info.SetHitEffecByPart(effect.Value.poolManager, effect.Value.hitEffectOffset);
			}
			
			enemy.Hit(info);
		}
	}

	public void Charging()
	{
		chargeEffect = proccessor.chargeEffectObjectPool.ActiveObject(proccessor.chargeEffectPos.position, proccessor.chargeEffectPos.rotation);
	}

	public void PreAttack()
	{
		currentCollider.ColliderReference.enabled = true;

		FDebug.Log("Pre : " + currentTime);
	}

	public void Attack()
	{
		FDebug.Log("Attack : " + currentTime);

		Vector3 vec = new Vector3(proccessor.explosionEffectPos.position.x, initialYPosition, proccessor.explosionEffectPos.position.z);

		explosionEffect = proccessor.explosionEffectObjectPool.ActiveObject(vec, Quaternion.identity);
		explosionEffect.GetComponent<ParticleController>().Initialize(proccessor.explosionEffectObjectPool);
		//proccessor.chargeEffectObjectPool.DeactiveObject(chargeEffect);

		float diameter = 2 * minSize * explosionEffectUnitSize;
		explosionEffect.localScale = new Vector3(diameter, diameter, diameter);
		currentTime = 0;

		currentCollider.transform.position = explosionEffect.transform.position;

		isExplosion = true;
	}

	// Player Landing 시, 데미지 처리
	public void Landing()
	{
		proccessor.landingEffectObjectPool.ActiveObject(proccessor.landingEffectPos.position, proccessor.landingEffectPos.rotation).
			GetComponent<ParticleController>().Initialize(proccessor.landingEffectObjectPool);
		currentCollider.transform.localPosition = Vector3.zero;
		EndExtension(pc);
	}

	public void AttackEnd()
	{
		currentCollider.transform.localPosition = Vector3.zero;
		pc.ChangeState(PlayerState.Idle);
	}
}
