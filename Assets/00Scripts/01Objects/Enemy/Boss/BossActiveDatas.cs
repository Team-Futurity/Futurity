using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossActiveDatas", menuName = "ScriptableObject/Boss/BossActiveDatas")]
public class BossActiveDatas : ScriptableObject
{
	public List<BossActiveData> activeDatas = new List<BossActiveData>();

	public float GetAfterDelayValue(Phase phase, BossController.BossState skillType)
	{
		float value = 0f;

		foreach (var delayValue in activeDatas)
			if (delayValue.phase == phase && delayValue.skillType == skillType)
				value = delayValue.skillAfterDelay;

		return value;
	}

	public float GetActivateDelayValue(BossController.BossState skillType)
	{
		float value = 0f;

		foreach (var delayValue in activeDatas)
			if (delayValue.skillType == skillType)
				value = delayValue.skillActivateDelay;

		return value;
	}
}