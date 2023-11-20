using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateData", menuName = "ScriptableObject/Unit/StateData")]
public class ChargeStateData : StateData
{
	// Constants
	public ChargeIncreases[] IncreasesByLevel;

	[Header("��Ÿ ����")]
	[Tooltip("�ִ� ���� �ܰ�")]								public int MaxLevel = 4;						// �ִ� ���� �ܰ�
	[Tooltip("Range ����Ʈ�� 1unit�� �ش��ϴ� Z�� ũ��")]	public float RangeEffectUnitLength = 0.145f;	// Range ����Ʈ�� 1unit�� �ش��ϴ� Z�� ũ��
	[Tooltip("���� ü�� ��")]								public float FlyPower = 45;						// ���� ü�� ��
	[Tooltip("�� �浹 �� ������")]							public float WallCollisionDamage = 50f;         // �� �浹 �� ������
	[Tooltip("��¡ �� �̵� �ӵ�")]							public float MoveSpeedInCharging = 0.5f;         // �� �浹 �� ������
	[Tooltip("�� �浹 ����Ʈ")]								public GameObject ChargeCollisionEffect;         // �� �浹 ����Ʈ
	[Tooltip("����Ʈ ��ġ ������")]							public float EffectPosOffset;
	[Tooltip("����Ʈ ȸ�� ������")]							public float EffectRotOffset;

	[Header("�� �浹 ������")]
	public ChargeCollisionData ChargeCollisionData;

	[Header("����")]
	public EventReference ChargeSound;
	public EventReference DashSound;
	public EventReference AttackSound;
	public EventReference WallSound;

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
		PlayerAttackState_Charged.MoveSpeedInCharging = MoveSpeedInCharging;
		PlayerAttackState_Charged.ChargeCollisionData = ChargeCollisionData;
		PlayerAttackState_Charged.ChargeCollisionEffect = ChargeCollisionEffect;

		PlayerAttackState_Charged.ChargeSound = ChargeSound;
		PlayerAttackState_Charged.DashSound = DashSound;
		PlayerAttackState_Charged.AttackSound = AttackSound;
		PlayerAttackState_Charged.WallSound = WallSound;
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
	[Tooltip("�ܰ� ���� �ð�")]				public float LevelStandard;
	[Tooltip("�ܰ�� ���� �Ÿ� ������")]	public float LengthMarkIncreasing;    
	[Tooltip("�ܰ�� �˹� �Ÿ� ������")]	public float KnockbackIncreasing;      
	[Tooltip("�ܰ�� ���� ���� ������")]	public float AttackSTIncreasing;
}