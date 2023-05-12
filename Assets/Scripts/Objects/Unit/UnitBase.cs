using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
	public UnitStatus status;
	
	protected virtual float GetCritical()
	{
		return UnityEngine.Random.Range(0, 1) < status.GetStatus(StatusName.CRITICAL_CHANCE) ? status.GetStatus(StatusName.CRITICAL_DAMAGE_MULTIPLIER) : 1;
	}

	protected abstract float GetAttakPoint(); // 최종 공격력을 반환
	protected abstract float GetDefensePoint(); // 최종 방어력 반환
	protected abstract float GetDamage(); // 최종 데미지 반환

	public abstract void Hit(UnitBase attacker, float damage); // Unit이 피격 됐을 때 호출
	public abstract void Attack(UnitBase target); // Unit이 공격할 때 호출

	public void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, transform.forward, Color.yellow);
	}
}
