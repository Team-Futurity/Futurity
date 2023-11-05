using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossActiveDatas", menuName = "ScriptableObject/Boss/BossActiveDatas")]
public class BossActiveDatas : ScriptableObject
{
	public List<BossAttackData> attackDatas = new List<BossAttackData>();

	#region Set Value
	public void SetCurAttackData(BossController unit)
	{
		Phase phase = unit.curPhase;
		BossState skillType = unit.nextState;

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
	public float GetAttackPointValue(BossController unit)
	{
		float value = 0f;
		Phase phase = unit.curPhase;
		BossState skillType = unit.curState;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.extraAttackPoint;

		return value;
	}
	public float GetPercentageValue(BossController unit)
	{
		int value = 0;
		Phase phase = Phase.Phase1;
		BossState skillType = unit.curState;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.percentage;

		return value;
	}
	public float GetAttackDelayValue(BossController unit)
	{
		float value = 0f;
		Phase phase = unit.curPhase;
		BossState skillType = unit.curState;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.attackDelay;

		return value;
	}

	public float GetAttackSpeedValue(BossController unit)
	{
		float value = 0f;
		Phase phase = unit.curPhase;
		BossState skillType = unit.curState;

		foreach (var data in attackDatas)
			if (data.phase == phase && data.state == skillType)
				value = data.attackSpeed;

		return value;
	}

	public float GetAfterDelayValue(BossController unit)
	{
		float value = 0f;
		Phase phase = unit.curPhase;
		BossState skillType = unit.curState;

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

		for(int i = 0; i<attackDatas.Count; i++)
		{
			if (attackDatas[i].phase == unit.curPhase)
			{
				sum += attackDatas[i].percentage;
				if(random < sum && attackDatas[i].state != unit.previousState && attackDatas[i].percentage > 0)
				{
					unit.nextState = attackDatas[i].state;
					break;
				}
			}
		}
	}
	#endregion
}