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
		// ���� enemy
		var hasTransitionComponent = enemy.TryGetComponent(out TransitionAttackCore transition);
			
		// ������ ���� ���� ���
		if (!hasTransitionComponent)
		{
			transition = enemy.AddComponent<TransitionAttackCore>();
		}

		// �����ϴٴ� ���� ���� ���� �ҷ��;� �Ѵ�.
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

		// ���͸� �����Ѵ�.
		var nearEnemyColliders = coll.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position)).ToList();

		// ��ȸ�Ѵ�.
		foreach (var nearEnemy in nearEnemyColliders)
		{
			// nearEnemy�� �Ѱܼ� ���ǿ� �������� Ȯ���Ѵ�.
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
