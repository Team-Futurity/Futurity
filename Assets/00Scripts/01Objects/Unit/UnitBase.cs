using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitBase : MonoBehaviour
{
	public StatusManager status;
	[SerializeField] protected Rigidbody rigid;

	public bool isGodMode = false;
	public bool isStun = false;
	[field: SerializeField] public bool IsAttackTime { get; private set; } // 현재 공격중인지
	[field: SerializeField] public bool IsAttackTiming { get; private set; } // 공격이 이뤄지는 시점이후인지

	[Header("런타임 변경 불가."), Tooltip("공격이 시작했는지를 체크할 고정 프레임(Fixed Delta Time) 단위")]
	public int AttackCheckFrameCount = 3;
	protected WaitForSeconds attackCheckWFS;
	protected Queue<DamageInfo> damageInfoQueue = new Queue<DamageInfo>();

	public UnityEvent<UnitBase> OnAttack;

	protected virtual void Start()
	{
		attackCheckWFS = new WaitForSeconds(Time.fixedDeltaTime * AttackCheckFrameCount);
		StartCoroutine(AttackProcessCorotutine());
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
	public abstract void Hit(UnitBase attacker, float damage, bool isDot = false); // Unit이 피격 됐을 때 호출

	public void Attack(UnitBase target, float attackST = 1)
	{
		DamageInfo info = new DamageInfo(this, target, attackST);
		damageInfoQueue.Enqueue(info);
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
	#endregion

	public virtual void Knockback(Vector3 direction, float power)
	{
		rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		rigid.AddForce(direction * power, ForceMode.Impulse);
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