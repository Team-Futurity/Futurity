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

		/*EditorGUILayout.LabelField("[����]��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������");
		chargeState = EditorGUILayout.FloatField()

		LengthMarkIncreasing = 200; // �ܰ�� ���� �Ÿ� ������
	public static float AttackSTIncreasing = 1;     // �ܰ�� ���� ���� ������
	public static float LevelStandard = 1;         // �ܰ踦 ���� ����
	public static int MaxLevel = 4;                 // �ִ� ���� �ܰ�
	public static float RangeEffectUnitLength = 0.145f; // Range ����Ʈ�� 1unit�� �ش��ϴ� Z�� ũ��
	public static float FlyPower = 45;               // ���� ü�� ��
	public static float WallCollisionDamage = 50f;*/
}
}
