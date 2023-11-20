using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct AlterAnimationData
{
	public int alterFrameCount;
	public int skipFrameCount;
	public int blendFrameCount;
	public float animationSpeed;

	public AlterAnimationData(int alterFrameCount, int skipFrameCount, float animationSpeed, int blendFrameCount)
	{
		this.alterFrameCount = alterFrameCount;
		this.skipFrameCount = skipFrameCount;
		this.animationSpeed = animationSpeed;
		this.blendFrameCount = blendFrameCount;
	}
}

public abstract class UnitBase : MonoBehaviour
{
	public StatusManager status;
	[SerializeField] protected Rigidbody rigid;

	public bool isGodMode = false;
	public bool isStun = false;
	[field: SerializeField] public bool IsAttackTime { get; private set; } // 현재 공격중인지
	[field: SerializeField] public bool IsAttackTiming { get; private set; } // 공격이 이뤄지는 시점이후인지

	[Header("참조 연결")]
	public Animator unitAnimator;

	[Header("런타임 변경 불가."), Tooltip("공격이 시작했는지를 체크할 고정 프레임(Fixed Delta Time) 단위")]
	public int AttackCheckFrameCount = 3;
	protected WaitForSeconds attackCheckWFS;
	protected Queue<DamageInfo> damageInfoQueue = new Queue<DamageInfo>();

	public UnityEvent<DamageInfo> onAttackEvent;

	private AlterAnimationData alterAnimationData;
	private bool isAlterSpeedForAnimation;

	public Image criticalImages;

	// collider
	private Collider[] colliders;
	private bool[] collidersAreEnabled;

	// boolean
	public bool isKnockbaking;

	protected virtual void Start()
	{
		if (unitAnimator == null) { FDebug.LogWarning("Animator is Null.", GetType()); }

		attackCheckWFS = new WaitForSeconds(Time.fixedDeltaTime * AttackCheckFrameCount);
		StartCoroutine(AttackProcessCorotutine());
		StartCoroutine(AnimationStopCoroutine());
		StartCoroutine(CheckKnockbackCoroutine());

		// colliders
		colliders = GetComponentsInChildren<Collider>();
		collidersAreEnabled = new bool[colliders.Length];
	}

	#region Getter
	protected virtual float GetCritical()
	{
		return UnityEngine.Random.Range(0f, 1f) < status.GetStatus(StatusType.CRITICAL_CHANCE).GetValue() ? status.GetStatus(StatusType.CRITICAL_DAMAGE_MULTIPLIER).GetValue() : 1;
	}

	protected abstract float GetAttackPoint(); // 최종 공격력을 반환
	protected abstract float GetDefensePoint(); // 최종 방어력 반환
	protected abstract float GetDamage(float damageValue); // 최종 데미지 반환
	#endregion

	#region Attack/Hit
	public abstract void Hit(DamageInfo damageInfo); // Unit이 피격 됐을 때 호출

	public void Attack(DamageInfo damageInfo)
	{
		DamageInfo info = new DamageInfo(damageInfo);
		damageInfoQueue.Enqueue(info);
	}
	public void InstantAttack(DamageInfo damageInfo)
	{
		DamageInfo info = new DamageInfo(damageInfo);
		AttackProcess(info);
	}

	protected abstract void AttackProcess(DamageInfo damageInfo); // Unit이 공격할 때 호출

	protected virtual IEnumerator AttackProcessCorotutine()
	{
		WaitForEndOfFrame eof = new WaitForEndOfFrame();
		while (true)
		{
			// 공격 중이 아닌 경우
			if (!IsAttackTime)
			{
				yield return attackCheckWFS; // 지정 시간 동안 대기
				continue;
			}

			// 공격 중인 경우
			// 공격이 이뤄지는 시점 이후인 경우
			if (IsAttackTiming)
			{
				// Queue에 들어온 게 있으면
				while (damageInfoQueue.Count > 0)
				{
					// Dequeue해서 Attack 실행
					var info = damageInfoQueue.Dequeue();
					info.Attacker.AttackProcess(info);
				}
			}

			yield return eof;
		}
	}

	public void HitEffectPooling(DamageInfo info)
	{
		if (info.HitEffectPoolManager == null) { return; }

		Transform effect;
		Vector3 rot = info.Defender.transform.rotation.eulerAngles;
		rot.y *= -1;
		effect = info.HitEffectPoolManager.ActiveObject(info.Defender.transform.position + info.HitEffectOffset, Quaternion.Euler(rot));
		var particles = effect.GetComponent<ParticleController>();

		if (particles != null)
		{
			particles.Initialize(info.HitEffectPoolManager);
		}
	}
	#endregion

	#region Production

	public void AlterAnimationSpeed(int alterFrameCount, int skipFrameCount, float animationSpeed, int blendFrameCount)
	{
		alterAnimationData.alterFrameCount = alterFrameCount;
		alterAnimationData.skipFrameCount = skipFrameCount;
		alterAnimationData.animationSpeed = animationSpeed;
		alterAnimationData.blendFrameCount = blendFrameCount;
		isAlterSpeedForAnimation = true;
	}

	public void AlterAnimationSpeed(AlterAnimationData data)
	{
		alterAnimationData = data;
		isAlterSpeedForAnimation = true;
	}

	public void RestoreOriginalAnimationSpeed()
	{
		alterAnimationData.alterFrameCount = 0;
		alterAnimationData.skipFrameCount = 0;
		alterAnimationData.animationSpeed = 1;
		isAlterSpeedForAnimation = false;
	}

	private IEnumerator AnimationStopCoroutine()
	{
		int currentStopedFrameCount = 0;
		int currentkipedFrameCount = 0;
		int currentBlentFrameCount = 0;
		while (true)
		{
			if (isAlterSpeedForAnimation)
			{
				if(unitAnimator != null)
				{
					currentkipedFrameCount = 0;
					while (isAlterSpeedForAnimation && currentkipedFrameCount < alterAnimationData.skipFrameCount)
					{
						yield return null;
						currentkipedFrameCount++;
					}

					currentBlentFrameCount = 0;
					while (isAlterSpeedForAnimation && currentBlentFrameCount < alterAnimationData.blendFrameCount)
					{
						yield return null;
						currentBlentFrameCount++;
						unitAnimator.speed = Mathf.Lerp(unitAnimator.speed, alterAnimationData.animationSpeed, currentBlentFrameCount / alterAnimationData.blendFrameCount);
					}

					currentStopedFrameCount = 0;
					unitAnimator.speed = alterAnimationData.animationSpeed;
					while (isAlterSpeedForAnimation && currentStopedFrameCount < alterAnimationData.alterFrameCount)
					{
						yield return null;
						currentStopedFrameCount++;
					}
					unitAnimator.speed = 1;
				}
				
				isAlterSpeedForAnimation = false;
			}

			yield return null;
		}
	}
	#endregion

	#region Collider
	public void DisableAllCollider()
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i] == null) { continue; }

			collidersAreEnabled[i] = colliders[i].enabled;
			colliders[i].enabled = false;
		}
	}

	public void RestoreCollider()
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i] == null) { continue; }

			colliders[i].enabled = collidersAreEnabled[i];
		}
	}
	#endregion

	public virtual void Knockback(Vector3 direction, float power)
	{
		rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		rigid.AddForce(direction * power, ForceMode.Impulse);
		isKnockbaking = true;
		FDebug.Log("활성화!");
	}
	protected IEnumerator CheckKnockbackCoroutine()
	{
		while (true)
		{
			if(isKnockbaking)
			{
				yield return null;
				if (rigid.velocity.magnitude < math.EPSILON)
				{
					isKnockbaking = false;
				}
			}
			yield return null;
		}
	}


	protected IEnumerator StartCriticalImage()
	{
		while(criticalImages.gameObject.activeSelf)
		{
			yield return new WaitForSeconds(1f);
			criticalImages.gameObject.SetActive(false);
		}
	}

	#region Switch(bool)
	// IsAttackTime을 true로 설정
	public void EnableAttackTime()
	{
		IsAttackTime = true;

		// Queue 청소
		if (damageInfoQueue.Count > 0) { damageInfoQueue.Clear(); }

		IsAttackTiming = false;
	}

	public void DisableAttackTime()
	{
		IsAttackTime = false;

		// Queue 청소
		if (damageInfoQueue.Count > 0) { damageInfoQueue.Clear(); }

		IsAttackTiming = false;
	}

	public void EnableAttackTiming()
	{
		IsAttackTime = true;
		IsAttackTiming = true;
	}
	#endregion

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, transform.forward, Color.yellow);
	}
#endif
}