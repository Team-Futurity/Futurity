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
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		ActivePartController controller = (ActivePartController)target;

		// PartController�� List<Part>�� ��ȸ�ϸ鼭 �� Part�� ���� Ŀ���� ������ ȣ��
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
			EditorGUILayout.EnumPopup("������ ����", part.stateToChange);

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
	}

	private void DrawTestInspector(TestActivePart part)
	{
		part.t1 = EditorGUILayout.FloatField("Test1", part.t1);
	}
	#endregion
}
