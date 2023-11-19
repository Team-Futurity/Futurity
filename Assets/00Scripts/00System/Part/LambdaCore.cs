using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LambdaCore : CoreAbility
{
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

	public GameObject effectPrefab;
	private GameObject effect;

	protected override void OnPartAbility(UnitBase enemy)
	{
	}

	private void Update()
	{
		if(!isActive || InputActionManager.Instance.currentActionMap != (InputActionMap)InputActionManager.Instance.InputActions.Player)
		{
			return;
		}

		timer += Time.deltaTime;

		if (timer >= colliderCheckCycle) 
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
			var obj = Instantiate(effectPrefab);
			obj.transform.position = enemy.transform.position;
			
			crowdSystem.SendCrowd(enemy.GetComponent<UnitBase>(), 0);
		}
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
