using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpecialMoveController))]
public class ActivePartControllerCustomEditor : Editor
{
	public const int activePartCount = 1;


	// GUI Contents
	GUIContent basicActivePartDuration = new GUIContent("공격 지속시간(sec)", "공격 범위가 [최소 반지름]에서 [최대 반지름]이 될 때까지의 시간");
	GUIContent basicActivePartBuffCode = new GUIContent("버프 코드", "범위 내에 들어온 적에게 부여할 상태 이상의 코드 값\n(BuffData Scriptable Object 참고)");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		SpecialMoveController controller = (SpecialMoveController)target;
		var list = controller.activePartDatas;
		var listCount = list.Count;
		var lastIndex = listCount - 1;

		// PartController의 List<Part>를 순회하면서 각 Part에 대한 커스텀 에디터 호출
		for (int i = 0; i < list.Count; i++)
		{
			var part = list[i].proccessor;
			var type = GetPartType(list[i].type);
			

			if (part != null)
			{
				if(part.GetType() != type)
				{
					part = null;
				}
			}

			if(type == null ) { continue; }
			
			if(part == null)
			{
				part = Activator.CreateInstance(type) as SpecialMoveProcessor;
				list[i].proccessor = part;
			}


			// Draw Inspector
			EditorGUILayout.LabelField($"{i}_{list[i].type} Part");
			EditorGUI.indentLevel++;
			part.stateToChange = (PlayerState)EditorGUILayout.EnumPopup("전이할 상태", part.stateToChange);

			DrawInspectorInPart(part);

			EditorGUI.indentLevel--;
		}

		if(list[lastIndex - 1].proccessor == list[lastIndex].proccessor)
		{
			var type = GetPartType(list[lastIndex].type);
			list[lastIndex].proccessor = Activator.CreateInstance(type) as SpecialMoveProcessor;
		}
	}

	private Type GetPartType(SpecialMoveType type)
	{
		switch (type)
		{
			case SpecialMoveType.Basic:
				return typeof(BasicActivePart);
			case SpecialMoveType.Test:
				return typeof(TestActivePart);
			case SpecialMoveType.Beta:
				return typeof(BetaActivePart);
			default:
				return null;
		}
	}


	private void DrawInspectorInPart(SpecialMoveProcessor part)
	{
		// Part 타입에 따라 해당 Part의 커스텀 에디터 호출

		switch (part)
		{
			case BasicActivePart basic:
				DrawBasicInspector(basic);
				break;
			case TestActivePart test:
				DrawTestInspector(test);
				break;
			case BetaActivePart beta:
				DrawBetaInspector(beta);
				break;
		}
	}

	#region ListInspector
	private void DrawBasicInspector(BasicActivePart part)
	{
		part.minRange = EditorGUILayout.FloatField("최소 반지름(cm)", part.minRange);
		part.maxRange = EditorGUILayout.FloatField("최대 반지름(cm)", part.maxRange);
		part.damage = EditorGUILayout.FloatField("피해량(절댓값)", part.damage);
		part.duration = EditorGUILayout.FloatField(basicActivePartDuration, part.duration);
		part.buffCode = EditorGUILayout.IntField(basicActivePartBuffCode, part.buffCode);
	}

	private void DrawTestInspector(TestActivePart part)
	{
		part.t1 = EditorGUILayout.FloatField("Test1", part.t1);
	}

	private void DrawBetaInspector(BetaActivePart part)
	{
		part.firstMaxAngle = EditorGUILayout.FloatField("최소 반지름(cm)", part.firstMaxAngle);
		part.firstRadius = EditorGUILayout.FloatField("최소 반지름(cm)", part.firstRadius);
		part.firstDamage = EditorGUILayout.FloatField("최소 반지름(cm)", part.firstDamage);
		part.firstDuration = EditorGUILayout.FloatField("최소 반지름(cm)", part.firstDuration);
		
		EditorGUILayout.Space(10);

		part.secondMaxAngle = EditorGUILayout.FloatField("최소 반지름(cm)", part.secondMaxAngle);
		part.secondRadius = EditorGUILayout.FloatField("최소 반지름(cm)", part.secondRadius);
		part.secondDamage = EditorGUILayout.FloatField("최소 반지름(cm)", part.secondDamage);
		part.secondDuration = EditorGUILayout.FloatField("최소 반지름(cm)", part.secondDuration);
		
		EditorGUILayout.Space(10);

		part.thirdMaxWdith = EditorGUILayout.FloatField("최소 반지름(cm)", part.thirdMaxWdith);
		part.thirdMaxHeight = EditorGUILayout.FloatField("최소 반지름(cm)", part.thirdMaxHeight);
		part.thirdDamage = EditorGUILayout.FloatField("최소 반지름(cm)", part.thirdDamage);
		part.thirdDuration = EditorGUILayout.FloatField("최소 반지름(cm)", part.thirdDuration);
	}
	#endregion
}
