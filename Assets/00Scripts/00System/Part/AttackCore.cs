using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	// Collider 생성 지름
	public float colliderRadius = .0f;
	
	// 직접 데미지를 주는 경우
	public float attackDamage = .0f;

	// 공격 시, 전이 효과
	[SerializeField]
	private bool isStateTransition;

	// 전이 효과가 존재할 경우, 충돌 Circle 사이즈
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	
	// Monster Data
	private Dictionary<int, UnitBase> hitEnemyDic = new Dictionary<int, UnitBase>();
	
	// Debug Mode
	public bool isDebugMode = false;
	public float debugColliderRadius = .0f;
	
	protected override void OnPartAbility(Enemy enemy)
	{
		switch (attackType)
		{
			case AttackCoreType.ADD_DAMAGE:
				AttackByDamage(enemy);
				break;
			
			case AttackCoreType.ADD_ODD_STATE:
				AttackByOddState(enemy);
				break;
		}
		
		hitEnemyDic.Clear();
	}

	private void AttackByDamage(Enemy enemy)
	{
		// 타격한 몬스터를 때린다.
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);
		enemyUnit.Hit(null, attackDamage);
		
		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit);

		// 주변으로 Collider를 그려준다.
		var coll = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);

		if (isStateTransition)
		{
			// 가장 가까운 몬스터 판별
			var nearEnemy = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList()[0];
			AttackHitEnemyDic(nearEnemy);
		}
		else
		{
			// 범위 안에 있는 몬스터를 공격한다.
			AddDamageEnemysInCollider(coll);
		}
	}

	// 범위 안에 있는 몬스터를 공격
	private void AddDamageEnemysInCollider(Collider[] enemyCollider)
	{
		foreach (var enemy in enemyCollider)
		{
			AttackHitEnemyDic(enemy);
		}
	}

	private void AttackHitEnemyDic(Collider enemy)
	{
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);
		enemyUnit.Hit(null, attackDamage);
		
		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit);
	}
	
	
	private void AttackByOddState(Enemy enemy)
	{
		
	}
	
	#region Debug

	private void OnDrawGizmos()
	{
		if (!isDebugMode)
		{
			return;
		}

		foreach (var hitEnemy in hitEnemyDic)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(hitEnemy.Value.transform.position, debugColliderRadius);
		}
	}
	
	#endregion
	
}
