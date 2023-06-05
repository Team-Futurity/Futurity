using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActivePartController))]
public class ActivePartControllerCustomEditor : Editor
{
	public const int activePartCount = 1;


	// GUI Contents
	GUIContent basicActivePartDuration = new GUIContent("공격 지속시간(sec)", "공격 범위가 [최소 반지름]에서 [최대 반지름]이 될 때까지의 시간");
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		ActivePartController controller = (ActivePartController)target;

		// PartController의 List<Part>를 순회하면서 각 Part에 대한 커스텀 에디터 호출
		for (int i = 0; i < controller.activePartDatas.Count; i++)
		{
			var part = controller.activePartDatas[i].proccessor;
			var type = GetPartType(controller.activePartDatas[i].type);
			

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
				part = Activator.CreateInstance(type) as ActivePartProccessor;
				controller.activePartDatas[i].proccessor = part;
			}


			// Draw Inspector
			EditorGUILayout.LabelField($"{i}_{controller.activePartDatas[i].type} Part");
			EditorGUI.indentLevel++;
			EditorGUILayout.EnumPopup("전이할 상태", part.stateToChange);

			DrawInspectorInPart(part);

			EditorGUI.indentLevel--;
		}
	}

	private Type GetPartType(ActivePartType type)
	{
		switch (type)
		{
			case ActivePartType.Basic:
				return typeof(BasicActivePart);
			case ActivePartType.Test:
				return typeof(TestActivePart);
			default:
				return null;
		}
	}


	private void DrawInspectorInPart(ActivePartProccessor part)
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
		}
	}

	#region ListInspector
	private void DrawBasicInspector(BasicActivePart part)
	{
		part.minRange = EditorGUILayout.FloatField("최소 반지름(cm)", part.minRange);
		part.maxRange = EditorGUILayout.FloatField("최대 반지름(cm)", part.maxRange);
		part.damage = EditorGUILayout.FloatField("피해량(절댓값)", part.damage);
		part.duration = EditorGUILayout.FloatField(basicActivePartDuration, part.duration);
	}

	private void DrawTestInspector(TestActivePart part)
	{
		part.t1 = EditorGUILayout.FloatField("Test1", part.t1);
	}
	#endregion
}
