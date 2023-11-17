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

	[field: SerializeField]
	public CrowdSystem crowdSystem { get; private set; }

	public GameObject effectPrefab;
	private GameObject effect;

	protected override void OnPartAbility(UnitBase enemy)
	{
		isActive = true;

		effect = Instantiate(effectPrefab, transform.position, transform.rotation);
		effect.transform.eulerAngles = new Vector3(-90, effect.transform.eulerAngles.y, 0);
	}

	private void OnDisable()
	{
		isActive = false;
		Destroy(effect);
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
			Destroy(effect);
			ExploreEnemy();
		}
	}

	// 몬스터 판별 -> 시점
	private void ExploreEnemy()
	{
		var catchEnemies = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);

		foreach (var enemy in catchEnemies)
		{
			crowdSystem.SendCrowd(enemy.GetComponent<UnitBase>(), 0);
		}
		Debug.Log("판별 완료!");
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
