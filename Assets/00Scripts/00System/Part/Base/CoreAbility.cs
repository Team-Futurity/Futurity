using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackCoreType
{
	NONE = 0,
	
	ADD_DAMAGE,
	ADD_ODD_STATE,
	
	MAX
}

public abstract class CoreAbility : MonoBehaviour
{
	// Core Type : Attack, Collider
	// Attack Type : Status or Damage

	// 상태 이상 : Buff System 적극 활용

	// Attack
	// 직접 어택, PlayerController Update를 받아서

	// Core 
	// Player Controller
	// - 몬스터 직접 어택 - 감마, 로우
	// - 사망한 몬스터 공격 범위 생성 

	// Collider
	// - 콜라이더 생성 ( 반지름, 몬스터 피해량, 형태, 시간 ) -? 전이 가능 형태??
	
	// protected Buff

	public bool isActive = false;

	public void ExecutePart(UnitBase enemy)
	{
		OnPartAbility(enemy);
	}

	protected abstract void OnPartAbility(UnitBase enemy);
}