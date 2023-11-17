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
	private int cycleCount = 0;

	private bool isActive = false;

	public ParticleSystem enableEffect;

	private void Update()
	{
		if(!isActive)
		{
			return;
		}

		timer += Time.deltaTime;

		if(timer >= cycleCount * enemyCheckCycle + enemyCheckCycle)
		{
			cycleCount++;

			var catchEnemys = PartCollider.DrawCircleCollider(transform.position, colliderRadius, targetLayer);
			

			if(catchEnemys.Length > 0)
			{
				enableEffect.gameObject.SetActive(false);
				enableEffect.gameObject.SetActive(true);
				//enableEffect.Play();
			}

			foreach (var enemy in catchEnemys)
			{
				
			}

			if(timer >= colliderActiveTIme)
			{
				Destroy(enableEffect);
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

	public void AddEnemyEvent(GameObject enableEffect)
	{
		var effect = Instantiate(enableEffect, transform.position, transform.rotation, transform);
		this.enableEffect = effect.GetComponent<ParticleSystem>();
		this.enableEffect.Pause();

		// Game으로 올린다.
		transform.parent = GameObject.Find("Game").transform;

		isActive = true;
	}

	private void CreateEnemyCollider()
	{
		
	}

}
