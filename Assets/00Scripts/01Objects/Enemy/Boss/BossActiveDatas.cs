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

		foreach (var data in activeDatas)
			if (data.phase == phase && data.skillType == skillType)
				value = data.skillAfterDelay;

		return value;
	}

	public float GetActivateDelayValue(BossController.BossState skillType)
	{
		float value = 0f;

		foreach (var data in activeDatas)
			if (data.skillType == skillType)
				value = data.skillActivateDelay;

		return value;
	}
}