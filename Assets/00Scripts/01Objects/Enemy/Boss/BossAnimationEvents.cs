using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
	public BossController bc;

	#region effect methods
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
		List<EffectActiveData> dataList = bc.attackEffectDatas;
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
	#endregion

	#region Activate Attack

	public void ActiveDash()
	{
		bc.rigid.velocity = bc.transform.forward.normalized * bc.bossData.status.GetStatus(StatusType.DASH_SPEED).GetValue();
	}
	public void ActivateType1Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.attackTrigger.type1Collider.SetActive(true);
	}
	public void ActivateType2Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.attackTrigger.type2Collider.SetActive(true);

	}
	public void ActivateType3Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.attackTrigger.ActiveListAttacks(bc.attackTrigger.type3Colliders);
	}

	public void ActiveType4Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.attackTrigger.ActiveListAttacks(bc.attackTrigger.type4Colliders);
	}
	public void ActiveType5Attack()
	{
		bc.attackTrigger.type5Manager.SpawnEnemy();
	}

	public void ActiveType6Attack()
	{
		bc.bossData.EnableAttackTiming();
		bc.attackTrigger.ActiveListAttacks(bc.attackTrigger.type6Colliders);
	}
	public void Type6RandomVector()
	{
		bc.attackTrigger.SetT6RandomVector(bc);
		bc.SetListEffectData(bc.attackEffectDatas, bc.attackTrigger.type6Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Target, false);
	}
	#endregion

	#region DeActive Attack
	public void DeActiveType1Attacks()
	{
		bc.attackTrigger.type1Collider.SetActive(false);
	}
	public void DeActiveType2Attacks()
	{
		bc.attackTrigger.type2Collider.SetActive(false);
	}
	public void DeActiveType3Attacks()
	{
		bc.attackTrigger.DeActiveListAttacks(bc.attackEffectDatas, bc.attackTrigger.type3Colliders);
	}
	public void DeActiveType4Attacks()
	{
		bc.attackTrigger.DeActiveListAttacks(bc.attackEffectDatas, bc.attackTrigger.type4Colliders);
	}

	public void DeActiveType6Attacks()
	{
		bc.attackTrigger.DeActiveListAttacks(bc.attackEffectDatas, bc.attackTrigger.type6Colliders);
	}
	#endregion

	#region Sound



	#endregion
}
