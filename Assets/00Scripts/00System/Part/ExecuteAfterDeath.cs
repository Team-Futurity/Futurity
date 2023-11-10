using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteAfterDeath : MonoBehaviour
{
	private float colliderRadius = .0f;
	private float enemyCheckCycle = .0f;
	private float colliderActiveTIme = .0f;

	private LayerMask targetLayer;

	private float timer = .0f;

	private bool isActive = false;

	private void Update()
	{
		if(!isActive)
		{
			return;
		}

		timer += Time.deltaTime;

		if((timer / enemyCheckCycle) >= enemyCheckCycle)
		{
			var catchEnemys = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);

			if(timer >= colliderActiveTIme)
			{
				Destroy(this.gameObject);
			}
		}
	}

	public void SetCollider(float radius, float checkCycle, float activeTime, LayerMask target)
	{
		colliderRadius = radius;
		enemyCheckCycle = checkCycle;
		colliderActiveTIme = activeTime;
		targetLayer = target;
	}

	public void AddEnemyEvent()
	{
		// Game���� �ø���.
		transform.parent = GameObject.Find("Game").transform;

		isActive = true;
	}

	private void CreateEnemyCollider()
	{
		
	}

}
