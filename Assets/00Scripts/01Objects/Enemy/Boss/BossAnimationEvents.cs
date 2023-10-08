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

	#region Activate Attack
	public void ActivateType1Attack()
	{
		bc.Type1Collider.SetActive(true);
	}
	public void ActivateType2Attack()
	{
		bc.Type2Collider.SetActive(true);
	}
	#endregion
}
