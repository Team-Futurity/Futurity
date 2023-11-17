using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCore : CoreAbility
{
	[field: SerializeField] public CrowdSystem crowdSystem { get; private set; }
	
	public AttackCoreType attackType;

	public LayerMask targetLayer;

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

	public GameObject effect;

	public LineRenderer transition;

	public Transform enemyParent;

	protected override void OnPartAbility(UnitBase enemy)
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

	private void AttackByDamage(UnitBase enemy)
	{
		var coll = PartCollider.DrawCircleCollider(enemy.transform.position, colliderRadius, targetLayer);
		//effect = Instantiate(effectPrefab, enemy.transform.position, enemy.transform.rotation);
		if (isStateTransition)
		{
			// ����� ������� ���͸� �����Ѵ�.
			var nearEnemies = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList();

			if(nearEnemies.Count == 0) { return; }

			var nearEnemy = nearEnemies[0];

			var hasTransitionComponent =
				nearEnemy.TryGetComponent<TransitionAttackCore>(out var transitionAttackCore);

			if (!hasTransitionComponent)
			{
				transitionAttackCore = nearEnemy.AddComponent<TransitionAttackCore>();
			}

			var transObj = Instantiate<LineRenderer>(transition, transform.position, Quaternion.identity, enemyParent);

			transitionAttackCore.SetTransitionData(new TransitionProtocol(
				id: ++transitionAttackID,
				radius: transitionColliderRadius,
				damage: transitionDamage,
				count : transitionCount,
				layer : targetLayer,
				render : transObj
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

		enemyUnit.Hit(new DamageInfo(
			attacker: null,
			defender: enemyUnit,
			attackST: attackDamage
			));

		if(effect != null)
			Instantiate(effect, enemyUnit.transform.position, Quaternion.identity);
		
		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit.gameObject);
	}

	private void AttackByOddState(UnitBase enemy)
	{
		crowdSystem.SendCrowd(enemy, 0);
	}
}