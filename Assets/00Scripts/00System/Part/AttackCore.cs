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

	// Collider 생성 지름
	public float colliderRadius = .0f;

	// 직접 데미지를 주는 경우
	public float attackDamage = .0f;

	// 공격 시, 전이 효과
	public bool isStateTransition;

	// 전이 효과가 존재할 경우, 충돌 Circle 사이즈
	public float transitionColliderRadius = .0f;
	public float transitionDamage = .0f;
	public int transitionCount = 0;
	private int transitionAttackID = 0;

	// Monster Data
	private Dictionary<int, GameObject> hitEnemyDic = new Dictionary<int, GameObject>();

	[SerializeField]private GameObject effectPrefab;
	private GameObject effect;

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
			// 가까운 순서대로 몬스터를 정리한다.
			var nearEnemies = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList();

			if(nearEnemies.Count == 0) { return; }

			var nearEnemy = nearEnemies[0];

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
			// 범위 안에 있는 몬스터를 공격한다.
			AttackAllEnemy(coll);
		}
	}

	// 범위 안에 있는 몬스터를 공격
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
		
		hitEnemyDic.Add(enemyUnit.GetInstanceID(), enemyUnit.gameObject);
	}

	private void AttackByOddState(UnitBase enemy)
	{
		crowdSystem.SendCrowd(enemy, 0);
	}
}