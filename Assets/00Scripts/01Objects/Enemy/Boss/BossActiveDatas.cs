using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossActiveDatas", menuName = "ScriptableObject/Boss/BossActiveDatas")]
public class BossActiveDatas : ScriptableObject
{
	public List<BossAttackData> attackDatas = new List<BossAttackData>();

	#region Set Value
	public void SetTypePercentage(BossController unit, Phase phase, BossState skillType)
	{
		if (unit.curAttackData == null)
			unit.curAttackData = new BossAttackData();

		BossAttackData data = new BossAttackData();
		foreach (var origin in attackDatas)
			if (origin.phase == phase && origin.state == skillType)
				data = origin;
		unit.curAttackData = data;
	}


	#endregion

	#region Get Value
	public float GetAttackPointValue(Phase phase, BossState skillType)
	{
		float value = 0f;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.extraAttackPoint;

		return value;
	}
	public float GetPercentageValue(Phase phase, BossState skillType)
	{
		int value = 0;
		phase = Phase.Phase1;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.percentage;

		return value;
	}
	public float GetAttackDelayValue(Phase phase, BossState skillType)
	{
		float value = 0f;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.attackDelay;

		return value;
	}

	public float GetAttackSpeedValue(Phase phase, BossState skillType)
	{
		float value = 0f;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.attackSpeed;

		return value;
	}

	public float GetAfterDelayValue(Phase phase, BossState skillType)
	{
		float value = 0f;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.attackAfterDelay;

		return value;
	}
	#endregion

	#region Random Value

	public void SetRandomNextState(BossController unit)
	{
		int sum = 0;
		int random = Random.Range(1, 101);

		foreach (var data in attackDatas)
		{
			if(data.phase == unit.curPhase)
			{
				sum += data.percentage;
				if (random < sum)
				{
					while (data.state == unit.previousState)
						SetRandomNextState(unit);
					
					unit.nextState = data.state;
				}
			}
		}
	}
	#endregion
}