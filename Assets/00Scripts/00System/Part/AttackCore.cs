using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCore : CoreAbility
{
	// ����, �ο�, �۽Ƿ�, 
	// ���� : �÷��̾� ���� ��, ���� ����
	// �ο� : �÷��̾� ���� ��, ���� ������ ����
	// �۽Ƿ� : �÷��̾� ���� ��, �ִ� 6�� ���̵Ǵ� ������ ����
	
	
	// Attack Core Type
	// 1. ���� �̻� �ο�
	// 2. ���� ������
	[SerializeField]
	private AttackCoreType attackType;

	[SerializeField]
	private LayerMask targetLayer;
	
	// ���� ��, ���� ȿ��
	[SerializeField]
	private bool isStateTransition;

	public float attackDamage = .0f;
	public float attackColliderRadius = .0f;
	
	// ���� ȿ���� ������ ���, �浹 Circle ������
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	
	// Monster Data
	private List<int> hitEnemyList = new List<int>();
	
	protected override void OnPartAbility(Enemy enemy)
	{
		// Attack ��, Buff ����
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
