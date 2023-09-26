using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

	[SerializeField]
	private LayerMask targetLayer;
	
	// 공격 시, 전이 효과
	[SerializeField]
	private bool isStateTransition;

	public float attackDamage = .0f;
	public float attackColliderRadius = .0f;
	
	// 전이 효과가 존재할 경우, 충돌 Circle 사이즈
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	
	// Monster Data
	private List<int> hitEnemyList = new List<int>();
	
	protected override void OnPartAbility(Enemy enemy)
	{
		// Attack 시, Buff 적용
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);

		enemyUnit.Hit(null, attackDamage);
		hitEnemyList.Add(enemyUnit.GetInstanceID());

		var coll = PartCollider.DrawCircleCollider(transform.position, attackColliderRadius, targetLayer);
		
		AttackEnemyInCollider(coll);
	}

	private void AttackEnemyInCollider(Collider[] enemyCollider)
	{
		foreach (var enemy in enemyCollider)
		{
			enemy.TryGetComponent<UnitBase>(out var enemyUnit);
			
			var alreadyHit = hitEnemyList.Contains(enemyUnit.GetInstanceID());
			
			if (alreadyHit)
			{
				continue;
			}
			
			enemyUnit.Hit(null, attackDamage);
			hitEnemyList.Add(enemyUnit.GetInstanceID());
		}
		
		hitEnemyList.Clear();
	}
}
