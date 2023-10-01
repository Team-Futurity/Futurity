using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Path;
using UnityEngine;

public class TransitionAttackCore : MonoBehaviour
{
	private TransitionProtocol protocol;

	public void SetTransitionData(TransitionProtocol receive)
	{
		protocol = receive;
	}

	public void Play(float delay = .0f)
	{
		PlayAction(delay);
	}

	public bool CompareID(int id)
	{
		return (protocol.transitionID == id);
	}

	private void PlayAction(float delay = .0f)
	{
		AddDamage();

		if (protocol.transitionCount > 0)
		{
			ExploreNearEnemy();
		}
	}

	private bool AddTransitionComponent(Collider enemy)
	{
		// 다음 enemy
		var hasTransitionComponent = enemy.TryGetComponent(out TransitionAttackCore transition);
			
		// 가지고 있지 않을 경우
		if (!hasTransitionComponent)
		{
			transition = enemy.AddComponent<TransitionAttackCore>();
		}

		// 동일하다는 것은 다음 것을 불러와야 한다.
		if (transition.CompareID(protocol.transitionID))
		{
			return false;
		}
		
		protocol.transitionCount--;

		transition.SetTransitionData(protocol);
		transition.PlayAction();
		
		return true;
	}

	private void ExploreNearEnemy()
	{
		var coll = PartCollider.DrawCircleCollider(transform.position, protocol.transitionColliderRadius, protocol.targetLayer);
		
		if (coll.Length <= 1)
		{
			return;
		}

		// 몬스터를 정렬한다.
		var nearEnemyColliders = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList();

		// 순회한다.
		foreach (var nearEnemy in nearEnemyColliders)
		{
			// nearEnemy를 넘겨서 조건에 부합한지 확인한다.
			var isFindNearEnemy = AddTransitionComponent(nearEnemy);

			if (isFindNearEnemy)
			{
				return;
			}
		}
	}

	private void AddDamage()
	{
		TryGetComponent(out Enemy enemyUnit);
		enemyUnit.Hit(null, protocol.attackDamage);
		Debug.Log($"ATTACK + " + transform.name);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, protocol.transitionColliderRadius);
	}
	
}
