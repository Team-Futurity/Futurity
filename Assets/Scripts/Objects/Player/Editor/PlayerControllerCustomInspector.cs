using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerController;

public class PlayerControllerCustomInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var pc = (PlayerController)target;
		UnitState<PlayerController> state = null;
		PlayerAttackState_Charged chargeState = null;

		/*pc.SetFSM();
		if(pc.GetState(PlayerState.ChargedAttack, ref state)) { return; }
		chargeState = (PlayerAttackState_Charged)state;*/

		/*EditorGUILayout.LabelField("[돌진]──────────────────────────────────────────────────────────────────────────────────────────────");
		chargeState = EditorGUILayout.FloatField()

		LengthMarkIncreasing = 200; // 단계당 돌진 거리 증가량
	public static float AttackSTIncreasing = 1;     // 단계당 공격 배율 증가량
	public static float LevelStandard = 1;         // 단계를 나눌 기준
	public static int MaxLevel = 4;                 // 최대 차지 단계
	public static float RangeEffectUnitLength = 0.145f; // Range 이펙트의 1unit에 해당하는 Z축 크기
	public static float FlyPower = 45;               // 공중 체공 힘
	public static float WallCollisionDamage = 50f;*/
}
}
