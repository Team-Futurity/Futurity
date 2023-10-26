using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateData", menuName = "ScriptableObject/Unit/StateData")]
public class ChargeStateData : StateData
{
	// Constants
	public ChargeIncreases[] IncreasesByLevel;

	[Tooltip("최대 차지 단계")]								public int MaxLevel = 4;						// 최대 차지 단계
	[Tooltip("Range 이펙트의 1unit에 해당하는 Z축 크기")]	public float RangeEffectUnitLength = 0.145f;	// Range 이펙트의 1unit에 해당하는 Z축 크기
	[Tooltip("공중 체공 힘")]								public float FlyPower = 45;						// 공중 체공 힘
	[Tooltip("벽 충돌 시 데미지")]							public float WallCollisionDamage = 50f;         // 벽 충돌 시 데미지

	public override void SetDataToState()
	{
		if (IncreasesByLevel.Length > MaxLevel)
		{
			IncreasesByLevel.CopyTo(IncreasesByLevel, MaxLevel);
		}

		var increases = SumPreviousIncreasing(IncreasesByLevel);

		PlayerAttackState_Charged.IncreasesByLevel = increases;
		PlayerAttackState_Charged.MaxLevel = MaxLevel;
		PlayerAttackState_Charged.RangeEffectUnitLength = RangeEffectUnitLength;
		PlayerAttackState_Charged.FlyPower = FlyPower;
		PlayerAttackState_Charged.WallCollisionDamage = WallCollisionDamage;
	}

	private ChargeIncreases[] SumPreviousIncreasing(ChargeIncreases[] increasesOrigin)
	{
		ChargeIncreases[] increases = new ChargeIncreases[increasesOrigin.Length];
		increasesOrigin.CopyTo(increases, 0);

		for(int i = 1; i < increases.Length; i++)
		{
			int preIndex = i - 1;
			increases[i].LevelStandard			+= increases[preIndex].LevelStandard;
			increases[i].LengthMarkIncreasing	+= increases[preIndex].LengthMarkIncreasing;
			increases[i].KnockbackIncreasing	+= increases[preIndex].KnockbackIncreasing;
			increases[i].AttackSTIncreasing		+= increases[preIndex].AttackSTIncreasing;
		}

		return increases;
	}
}


[Serializable]
public struct ChargeIncreases
{
	[Tooltip("단계 유지 시간")]				public float LevelStandard;
	[Tooltip("단계당 돌진 거리 증가량")]	public float LengthMarkIncreasing;    
	[Tooltip("단계당 넉백 거리 증가량")]	public float KnockbackIncreasing;      
	[Tooltip("단계당 공격 배율 증가량")]	public float AttackSTIncreasing;
}