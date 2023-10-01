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
	// �ο� : �÷��̾� ���� ��, ���� ������ ����
	// �۽Ƿ� : �÷��̾� ���� ��, �ִ� 6�� ���̵Ǵ� ������ ����

	// Attack Core Type
	// 1. ���� �̻� �ο�
	// 2. ���� ������
	public AttackCoreType attackType;

	[SerializeField]
	private LayerMask targetLayer;

	// Collider ���� ����
	public float colliderRadius = .0f;

	// ���� �������� �ִ� ���
	public float attackDamage = .0f;

	// ���� ��, ���� ȿ��
	public bool isStateTransition;

	// ���� ȿ���� ������ ���, �浹 Circle ������
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	private int transitionAttackID = 0;

	// Monster Data
	private Dictionary<int, GameObject> hitEnemyDic = new Dictionary<int, GameObject>();

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
		var coll = PartCollider.DrawCircleCollider(enemy.transform.position, colliderRadius, targetLayer);

		if (isStateTransition)
		{
			// ����� ������� ���͸� �����Ѵ�.
			var nearEnemy = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position))
				.ToList()[0];

			var hasTransitionComponent =
				nearEnemy.TryGetComponent<TransitionAttackCore>(out var transitionAttackCore);

			if (!hasTransitionComponent)
			{
				transitionAttackCore = nearEnemy.AddComponent<TransitionAttackCore>();
			}

			transitionAttackCore.SetTransitionData(new TransitionProtocol(
				id: ++transitionAttackID,
				radius: transitionColliderRadius,
				damage: transitionDamage,
				count : transitionCount,
				layer : targetLayer
				));
			
			transitionAttackCore.Play(1f);
		}
		else
		{
			// ���� �ȿ� �ִ� ���͸� �����Ѵ�.
			AttackAllEnemy(coll);
		}
	}

	// ���� �ȿ� �ִ� ���͸� ����
	private void AttackAllEnemy(Collider[] enemyCollider)
	{
		foreach (var enemy in enemyCollider)
		{
			AttackEnemy(enemy);
		}
	}

	private void AttackEnemy(Collider enemy)
	{
		enemy.TryGetComponent<UnitBase>(out var enemyUnit);

		if (hitEnemyDic.ContainsValue(enemy.gameObject))
		{
			return;
		}

		enemyUnit.Hit(null, attackDamage);

		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit.gameObject);
	}

	private void AttackByOddState(Enemy enemy)
	{
		
	}
}