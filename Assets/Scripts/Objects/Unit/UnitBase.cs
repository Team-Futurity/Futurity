using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    [Header("1차 능력치")]
    [Tooltip("현 체력")]
    [SerializeField] protected float currentHp = 200f;
    [Tooltip("최대 체력")]
    [SerializeField] protected float maxHp = 200f;
    [Tooltip("이동속도")]
    [SerializeField] protected float speed = 3f;
    [Tooltip("최종 공격치")]
    [SerializeField] protected float attack = 20f;
    [Tooltip("방어치")]
    [SerializeField] protected float defence = 5f;
    [Tooltip("크리티컬 확률")]
    [Range(0, 1)]
    [SerializeField] protected float criticalChance = 0f;
    [Tooltip("크리티컬 데미지 배율")]
    [SerializeField] protected float criticalDamageMultiplier = 0f;
    
    // Buff - Invincibility
    protected bool isGodMode = false;
    // Buff - Dot
    
    // Buff - Stun


    public float CurrentHp { get { return currentHp; } set { currentHp = value; } } 
    public float MaxHp => maxHp;
    public float Speed => speed;
    public float AttackPoint { get { return attack; } set { attack = value; } }
    public float DefencePoint => defence;
    public float CriticalChance { get { return criticalChance; } set { criticalChance = Mathf.Clamp(value, 0, 1); } }
    public float CriticalDamageMultiplier => criticalDamageMultiplier;

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

    public void SetSpeed(float changeSpeed)
    {
	    speed = changeSpeed;
    }

    public void SetGodMode(bool isTrigger)
    {
	    isGodMode = isTrigger;
    }

	public void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, transform.forward, Color.yellow);
	}
}
