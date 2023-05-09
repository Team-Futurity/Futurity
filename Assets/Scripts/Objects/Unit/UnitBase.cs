using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
	[field: SerializeField] public UnitStatus status { get; private set; }
	public float CurrentHp { get { return status.currentHp; } set { status.currentHp = value; } } 
    public float MaxHp => status.maxHp;
    public float Speed => status.speed;
    public float AttackPoint { get { return status.attack; } set { status.attack = value; } }
    public float DefencePoint => status.defence;
    public float CriticalChance { get { return status.criticalChance; } set { status.criticalChance = Mathf.Clamp(value, 0, 1); } }
    public float CriticalDamageMultiplier => status.criticalDamageMultiplier;

    // CriticalChance의 확률에 따라 데미지 계수가 CriticalDamageMultiplier 또는 1로 적용
    protected virtual float GetCritical()
    {
        return UnityEngine.Random.Range(0, 1) < CriticalChance ? CriticalDamageMultiplier : 1;
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
