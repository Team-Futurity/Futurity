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
	GUIContent basicActivePartDuration = new GUIContent("���� ���ӽð�(sec)", "���� ������ [�ּ� ������]���� [�ִ� ������]�� �� �������� �ð�");
	GUIContent basicActivePartBuffCode = new GUIContent("���� �ڵ�", "���� ���� ���� ������ �ο��� ���� �̻��� �ڵ� ��\n(BuffData Scriptable Object ����)");

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		ActivePartController controller = (ActivePartController)target;
		var list = controller.activePartDatas;
		var listCount = list.Count;
		var lastIndex = listCount - 1;

		// PartController�� List<Part>�� ��ȸ�ϸ鼭 �� Part�� ���� Ŀ���� ������ ȣ��
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
				part = Activator.CreateInstance(type) as ActivePartProccessor;
				list[i].proccessor = part;
			}


			// Draw Inspector
			EditorGUILayout.LabelField($"{i}_{list[i].type} Part");
			EditorGUI.indentLevel++;
			part.stateToChange = (PlayerState)EditorGUILayout.EnumPopup("������ ����", part.stateToChange);

			DrawInspectorInPart(part);

			EditorGUI.indentLevel--;
		}

		if(list[lastIndex - 1].proccessor == list[lastIndex].proccessor)
		{
			var type = GetPartType(list[lastIndex].type);
			list[lastIndex].proccessor = Activator.CreateInstance(type) as ActivePartProccessor;
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
		// Part Ÿ�Կ� ���� �ش� Part�� Ŀ���� ������ ȣ��

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
		part.minRange = EditorGUILayout.FloatField("�ּ� ������(cm)", part.minRange);
		part.maxRange = EditorGUILayout.FloatField("�ִ� ������(cm)", part.maxRange);
		part.damage = EditorGUILayout.FloatField("���ط�(����)", part.damage);
		part.duration = EditorGUILayout.FloatField(basicActivePartDuration, part.duration);
		part.buffCode = EditorGUILayout.IntField(basicActivePartBuffCode, part.buffCode);
	}

	private void DrawTestInspector(TestActivePart part)
	{
		part.t1 = EditorGUILayout.FloatField("Test1", part.t1);
	}
	#endregion
}
