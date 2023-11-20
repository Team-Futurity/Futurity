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
	private ParticleSystem effectSystem;

	protected override void OnPartAbility(UnitBase enemy)
	{
		effect.SetActive(true);
	}

	private void Awake()
	{
		effect = Instantiate(effectPrefab);
		TryGetComponent(out effectSystem);
		effect.SetActive(false);
	}

	private void Update()
	{
		if(!isActive || InputActionManager.Instance.currentActionMap != (InputActionMap)InputActionManager.Instance.InputActions.Player)
		{
			effect.SetActive(false);
			return;
		}

		timer += Time.deltaTime;
		var resultTrans = transform.position;
		resultTrans.y += 0.01f;
		
		effect.transform.position = resultTrans;
		
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
