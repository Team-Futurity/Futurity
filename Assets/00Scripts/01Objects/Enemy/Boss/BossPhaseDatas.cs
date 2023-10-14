using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPhaseDatas", menuName = "ScriptableObject/Boss/BossPhaseDatas")]
public class BossPhaseDatas : ScriptableObject
{
	public List<BossPhaseData> phaseDatas = new List<BossPhaseData>();

	#region get
	public float GetPhasePercentage(Phase phase)
	{
		float value = 0f;
		foreach (var maxTime in phaseDatas)
			if (maxTime.phase == phase)
				value = maxTime.hpPercentage;

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

	public int GetPercentage4(Phase phase)
	{
		int value = 0;
		foreach (var percentage in phaseDatas)
			if (percentage.phase == phase)
				value = percentage.type4Percentage;

		return value;
	}

	public int GetPercentage6(Phase phase)
	{
		int value = 0;
		foreach (var percentage in phaseDatas)
			if (percentage.phase == phase)
				value = percentage.type6Percentage;

		return value;
	}

	public int GetPercentage7(Phase phase)
	{
		int value = 0;
		foreach (var percentage in phaseDatas)
			if (percentage.phase == phase)
				value = percentage.type7Percentage;

		return value;
	}
	#endregion

	public BossController.BossState RandomState(Phase phase)
	{
		int random = Random.Range(0, 100);

		if (random < GetPercentage4(phase))
			return BossController.BossState.T4_Laser;
		else if (random < GetPercentage4(phase) + GetPercentage6(phase))
			return BossController.BossState.T6_Circle;
		else
			return BossController.BossState.T7_Trap;
	}
}
