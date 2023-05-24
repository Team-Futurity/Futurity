using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
	private EnemyController ec;

	private int curIndex;
	private bool effectActive = false;
	private float curTime = 0f;
	private float endTime = 0.8f;

	private void Start()
	{
		ec = GetComponent<EnemyController>();
	}

	private void Update()
	{
		if (effectActive && ec.initiateEffects != null)
		{
			curTime += Time.deltaTime;
			ec.initiateEffects[curIndex].SetActive(true);

			if (curTime > endTime)
			{
				ec.initiateEffects[curIndex].SetActive(false);
				effectActive = false;
			}
		}
	}

	public void EffectActive(int index)
	{
		/*		effect = ec.effectPoolManager.ActiveObject(ec.effectPos.position, ec.transform.rotation);
				effectPrefab.transform.rotation = ec.effectParent.transform.rotation;*/

		curIndex = index;
		effectActive = true;
		curTime = 0f;
	}
}