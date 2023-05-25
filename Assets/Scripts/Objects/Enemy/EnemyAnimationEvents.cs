using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
	private EnemyController ec;
	[HideInInspector] public Transform effect;

	private GameObject effectPrefab;

	private bool effectActive = false;
	private float curTime = 0f;
	private float endTime = 0.8f;

	private void Start()
	{
		ec = GetComponent<EnemyController>();
		if (effectPrefab == null)
		{
			//effectPrefab = GameObject.Instantiate(ec.effectPrefab, ec.effectParent == null ? null : ec.effectPos.transform);
			effectPrefab.SetActive(false);
		}
	}

	private void Update()
	{
		if(effectActive && effectPrefab != null)
		{
			curTime += Time.deltaTime;
			effectPrefab.SetActive(true);

			if(curTime > endTime)
			{
				effectPrefab.SetActive(false);
				effectActive = false;
			}
		}
	}

	public void PrintEffect()
	{
		//effect = ec.effectPoolManager.ActiveObject(ec.effectPos.position, ec.transform.rotation);
		//effectPrefab.transform.rotation = ec.effectParent.transform.rotation;
		effectActive = true;
		curTime = 0f;
	}
}
