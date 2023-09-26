using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCore : CoreAbility
{
	// 감마, 로우, 앱실론, 
	// 감마 : 플레이어 어택 시, 버프 적용
	// 로우 : 플레이어 어택 시, 범위 데미지 적용
	// 앱실론 : 플레이어 어택 시, 최대 6번 전이되는 데미지 적용
	
	
	// Attack Core Type
	// 1. 상태 이상 부여
	// 2. 직접 데미지
	[SerializeField]
	private AttackCoreType attackType;
	
	// 공격 시, 전이 효과
	[SerializeField]
	private bool isStateTransition;

	private float attackDamage = .0f;
	
	// 전이 효과가 존재할 경우, 충돌 Circle 사이즈
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	
	// Monster Data
	
	protected override void OnPartAbility(EnemyController enemy)
	{
		// Attack 시, Buff 적용
	}
	
}
