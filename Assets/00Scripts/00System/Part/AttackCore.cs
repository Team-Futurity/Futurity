using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCore : CoreAbility
{
	// ����, �ο�, �۽Ƿ�, 
	// ���� : �÷��̾� ���� ��, ���� ����
	// �ο� : �÷��̾� ���� ��, ���� ������ ���� -> Collider ���
	// �۽Ƿ� : �÷��̾� ���� ��, �ִ� 6�� ���̵Ǵ� ������ ���� -> Collider ���
	
	// Attack Core Type
	// 1. ���� �̻� �ο�
	// 2. ���� ������
	[SerializeField]
	private AttackCoreType attackType;

	[SerializeField]
	private LayerMask targetLayer;

	// Collider ���� ����
	public float colliderRadius = .0f;
	
	// ���� �������� �ִ� ���
	public float attackDamage = .0f;

	// ���� ��, ���� ȿ��
	[SerializeField]
	private bool isStateTransition;

	// ���� ȿ���� ������ ���, �浹 Circle ������
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
		// Ÿ���� ���͸� ������.
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);
		enemyUnit.Hit(null, attackDamage);
		
		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit);

		// �ֺ����� Collider�� �׷��ش�.
		var coll = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);

		if (isStateTransition)
		{
			for (int i = 0; i < coll.Length; ++i)
			{
				var nearEnemy = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList()[i];
				
				if(AttackHitEnemyDic(nearEnemy))
				{
					break;
				}
			}
		}
		else
		{
			// ���� �ȿ� �ִ� ���͸� �����Ѵ�.
			AddDamageEnemysInCollider(coll);
		}
	}

	// ���� �ȿ� �ִ� ���͸� ����
	private void AddDamageEnemysInCollider(Collider[] enemyCollider)
	{
		foreach (var enemy in enemyCollider)
		{
			AttackHitEnemyDic(enemy);
		}
	}

	private bool AttackHitEnemyDic(Collider enemy)
	{
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);

		if (hitEnemyDic.ContainsKey(enemyUnit.GetInstanceID()))
		{
			return false;
		}

		enemyUnit.Hit(null, attackDamage);
		
		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit);

		return true;
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
