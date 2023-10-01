using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Path;
using UnityEngine;

public class TransitionAttackCore : MonoBehaviour
{
	private float colliderRadius = .0f;
	private int attackCount = 0;
	private float attackDamage = .0f;
	private LayerMask targetLayer;

	private bool isSearchNextMonster = false;

	public void SetTransitionData(float radius, int count, float damage, LayerMask target)
	{
		Debug.Log($"Enemy Name {transform.name}");
		
		colliderRadius = radius;
		attackCount = count;
		attackDamage = damage;
		targetLayer = target;
	}

	public void PlayAction(float delay = .0f)
	{
		AddDamage();
		
		var nextEnemy = ExploreNearEnemy();

		if (attackCount <= 0 || nextEnemy == null)
		{
			return;
		}
		
		var hasTransitionComponent = nextEnemy.TryGetComponent(out TransitionAttackCore transitionAttackCore);

		if (!hasTransitionComponent)
		{
			transitionAttackCore = nextEnemy.gameObject.AddComponent<TransitionAttackCore>();
		}
			
		transitionAttackCore.SetTransitionData(colliderRadius, attackCount - 1, attackDamage, targetLayer);
		transitionAttackCore.PlayAction(1f);
	}

	private Collider ExploreNearEnemy()
	{
		var coll = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);
		
		if (coll.Length <= 1)
		{
			return null;
		}
		
		var nearEnemy = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList()[1];
		
		return nearEnemy;
	}

	private void AddDamage()
	{
		TryGetComponent(out Enemy enemyUnit);
		
		enemyUnit.Hit(null, attackDamage);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, colliderRadius);
	}
}
