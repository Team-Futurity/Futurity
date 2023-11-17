using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambdaCore : CoreAbility
{
	private bool isActive = false;

	[SerializeField, Header("�ݶ��̴� ����")]
	private float colliderRadius = .0f;

	[SerializeField, Header("���� �Ǻ� �ֱ�")]
	private float colliderCheckCycle = .0f;

	[SerializeField, Header("Ÿ�� ���̾�")]
	private LayerMask targetLayer;

	public bool isDebug = false;

	private float timer = .0f;

	[field: SerializeField]
	public CrowdSystem crowdSystem { get; private set; }

	protected override void OnPartAbility(UnitBase enemy)
	{
		isActive = true;
	}

	private void OnDisable()
	{
		isActive = false;
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

	// ���� �Ǻ� -> ����
	private void ExploreEnemy()
	{
		var catchEnemies = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);

		foreach (var enemy in catchEnemies)
		{
			crowdSystem.SendCrowd(enemy.GetComponent<UnitBase>(), 0);
		}
		Debug.Log("�Ǻ� �Ϸ�!");
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
