using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambdaCore : CoreAbility
{
	private bool isActive = false;

	[SerializeField, Header("콜라이더 범위")]
	private float colliderRadius = .0f;

	[SerializeField, Header("몬스터 판별 주기")]
	private float colliderCheckCycle = .0f;

	[SerializeField, Header("타겟 레이어")]
	private LayerMask targetLayer;

	public bool isDebug = false;

	private float timer = .0f;

	protected override void OnPartAbility(UnitBase enemy)
	{
		isActive = true;
	}

	private void Update()
	{
		if(!isActive)
		{
			return;
		}

		timer += Time.deltaTime;

		if(timer >= colliderCheckCycle)
		{
			timer = .0f;

			ExploreEnemy();
		}
	}

	// 몬스터 판별 -> 시점
	private void ExploreEnemy()
	{
		var catchEnemys = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);
		
	}

	private void OnDrawGizmos()
	{
		if (!isDebug)
		{
			return;
		}

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, colliderRadius);

	}

}
