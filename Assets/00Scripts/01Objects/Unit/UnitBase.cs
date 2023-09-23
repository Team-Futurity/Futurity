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
	public bool IsAttackTime { get; set; } // 현재 공격중인지
	public bool IsAttackTiming { get; set; } // 공격이 이뤄지는 시점이후인지

	[Header("런타임 변경 불가."), Tooltip("공격이 시작했는지를 체크할 고정 프레임(Fixed Delta Time) 단위")]
	public int AttackCheckFrameCount = 3;
	protected WaitForSeconds attackCheckWFS;
	//protected List<>

	public UnityEvent<UnitBase> OnAttack;

	protected virtual void Start()
	{
		attackCheckWFS = new WaitForSeconds(Time.fixedDeltaTime * AttackCheckFrameCount);
	}

	protected virtual IEnumerable AttackProcessCorotutine()
	{
		while(true)
		{
			if(IsAttackTime)
			{
				//if()
			}
			yield return attackCheckWFS;
		}
	}	

	protected virtual float GetCritical()
	{
		return UnityEngine.Random.Range(0f, 1f) < status.GetStatus(StatusType.CRITICAL_CHANCE).GetValue() ? status.GetStatus(StatusType.CRITICAL_DAMAGE_MULTIPLIER).GetValue() : 1;
	}

	protected abstract float GetAttackPoint(); // 최종 공격력을 반환
	protected abstract float GetDefensePoint(); // 최종 방어력 반환
	protected abstract float GetDamage(float damageValue); // 최종 데미지 반환

	public abstract void Hit(UnitBase attacker, float damage, bool isDot = false); // Unit이 피격 됐을 때 호출
	public abstract void Attack(UnitBase target, float attackST = 1); // Unit이 공격할 때 호출


	public virtual void Knockback(Vector3 direction, float power)
	{
		rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		rigid.AddForce(direction * power, ForceMode.Impulse);
	}

#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, transform.forward, Color.yellow);
	}
#endif
}