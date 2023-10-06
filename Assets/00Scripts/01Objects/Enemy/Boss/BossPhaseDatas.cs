using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhaseDatas", menuName = "ScriptableObject/Boss/BossPhaseDatas")]
public class BossPhaseDatas : ScriptableObject
{
	public List<BossPhaseData> phaseDatas = new List<BossPhaseData>();

	public float GetPhasePercentage(Phase phase)
	{
		float value = 0f;
		foreach (var maxTime in phaseDatas)
			if (maxTime.phase == phase)
				value = maxTime.percentage;

		return value;
	}

	public float GetType467TImerValue(Phase phase)
	{
		float value = 0f;
		foreach (var maxTime in phaseDatas)
			if (maxTime.phase == phase)
				value = maxTime.type467MaxTime;

		return value;
	}

	public float GetType5TImerValue(Phase phase)
	{
		float value = 0f;
		foreach (var maxTime in phaseDatas)
			if (maxTime.phase == phase)
				value = maxTime.type5MaxTime;

		return value;
	}
}
