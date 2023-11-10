using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhaseDatas", menuName = "ScriptableObject/Boss/BossPhaseDatas")]
public class BossPhaseDatas : ScriptableObject
{
	public List<BossPhaseData> phaseDatas = new List<BossPhaseData>();

	#region Get/Set Percentage Value
	public float GetHPPercentage(Phase phase)
	{
		float value = 0f;
		foreach (var data in phaseDatas)
			if (data.phase == phase)
				value = data.hpPercentage;

		return value * 0.01f;
	}

	#endregion
}
