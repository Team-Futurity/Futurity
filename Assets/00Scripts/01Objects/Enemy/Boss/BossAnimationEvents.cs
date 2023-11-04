using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
	public BossController bc;

	public void ActiveEffect(int activeIndex = 0)
	{
		EffectActiveData data = bc.currentEffectData;
		EffectKey key = bc.effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, activeIndex, false);

		var particles = key.EffectObject.GetComponent<ParticleActiveController>();

		if (particles != null)
		{
			particles.Initialize(bc.effectController, key);
		}
	}

	public void ActiveAllEffects()
	{
		List<EffectActiveData> dataList = bc.listEffectData;
		for (int index = 0; index < dataList.Count; index++)
		{
			EffectKey key = bc.effectController.ActiveEffect(dataList[index].activationTime, dataList[index].target, dataList[index].position, dataList[index].rotation, dataList[index].parent, dataList[index].index, dataList[index].index, true);
			var particles = key.EffectObject.GetComponent<ParticleActiveController>();
			if (particles != null)
			{
				particles.Initialize(bc.effectController, key);
			}
		}
	}

	public void Type6RandomVector()
	{
		int randx;
		int randz;
		for (int i = 0; i < bc.Type6Colliders.Count; i++)
		{
			randx = Random.Range(-6, 6);
			randz = Random.Range(-6, 6);
			bc.Type6Colliders[i].transform.position = new Vector3(randx, bc.transform.position.y, randz);
		}
		bc.SetEffectData(bc.Type6Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Target, true);
	}

	#region Activate Attack
	public void ActivateType1Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.Type1Collider.SetActive(true);
	}
	public void ActivateType2Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.Type2Collider.SetActive(true);

	}
	public void ActivateType3Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.ActiveAttacks(bc.Type3Colliders);
	}

	public void ActiveType4Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.ActiveAttacks(bc.Type4Colliders);
	}
	public void DeActiveType4Attacks()
	{
		bc.DeActiveAttacks(bc.Type4Colliders);
	}

	public void ActiveType5Attack()
	{
		bc.attackTrigger.type5Manager.SpawnEnemy();
	}
	public void ActiveType6Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.ActiveAttacks(bc.Type6Colliders);
	}
	public void DeActiveType6Attacks()
	{
		bc.DeActiveAttacks(bc.Type6Colliders);
	}
	public void ActiveType7Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.ActiveAttacks(bc.Type7Colliders);
	}

	#endregion
}
