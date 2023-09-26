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

	// ���� �������� �ִ� ���
	public float attackDamage = .0f;
	public float attackColliderRadius = .0f;

	// ���� ��, ���� ȿ��
	[SerializeField]
	private bool isStateTransition;

	// ���� ȿ���� ������ ���, �浹 Circle ������
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	
	// Monster Data
	private List<int> hitEnemyList = new List<int>();
	
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
	}

	private void AttackByDamage(Enemy enemy)
	{
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);
		
		enemyUnit.Hit(null, attackDamage);
		hitEnemyList.Add(enemyUnit.GetInstanceID());

		var coll = PartCollider.DrawCircleCollider(transform.position, attackColliderRadius, targetLayer);
		
		AddDamageEnemysInCollider(coll);
	}

	private void AttackByOddState(Enemy enemy)
	{
		
	}

	// ���� �ȿ� �ִ� ���͸� ����
	private void AddDamageEnemysInCollider(Collider[] enemyCollider)
	{
		foreach (var enemy in enemyCollider)
		{
			enemy.TryGetComponent<UnitBase>(out var enemyUnit);
			enemyUnit.Hit(null, attackDamage);
		}
	}
	
	// ���� �ȿ� �ִ� ���� ��, ���� ����� ���� ĳ��
}
