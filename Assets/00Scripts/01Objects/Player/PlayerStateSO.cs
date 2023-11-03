using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public enum StateType
{
	Enemy,
	Player
}

[CreateAssetMenu(fileName = "StateChangeConditions", menuName = "ScriptatbleObject/State/ChangeConditions")]
public class StateChangeConditions : ScriptableObject
{
	public StateType stateType;
	[SerializeField] public bool[,] conditions;

	public bool GetChangable(int current, int next)
	{
		int length = conditions.Length;

		if(current < 0 || current >= length) { return false; }
		if(next < 0 || next >= length) { return false; }

		return conditions[current, length - next];
	}
}

[CustomEditor(typeof(StateChangeConditions))]
public class StateChangeConditionsCustomEditor : Editor
{
	private int selectedEnumIndex = 0;
	private string[] enumNames;
	private int enumLength;

	private StateChangeConditions conditions;

	private void OnEnable()
	{
		conditions = target as StateChangeConditions;

		SetEnumNames();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (conditions == null) { return; }

		conditions = target as StateChangeConditions;
		SetEnumNames();

		var style = new GUIStyle();
		style.alignment = TextAnchor.MiddleRight;
		style.fixedHeight = 0;

		float toggleWidth = 20f;
		float labelWidth = 175f;
		float labelHeight = 250f;

		serializedObject.Update();

		GUILayout.BeginHorizontal();

		GUILayout.Space(labelWidth);
		Vector2 pos = GUILayoutUtility.GetLastRect().position;
		pos.y += labelWidth * 1.075f;
		pos.x += labelWidth * 0.725f;
		//Vector2 pos = new Vector2(labelWidth * 1.2f, 0.5f);
		for (int h = enumLength - 1; h >= 0; h--)
		{
			//GUILayoutUtility.GetLastRect().center;
			GUIUtility.RotateAroundPivot(90, pos);
			EditorGUILayout.LabelField(enumNames[h], style, GUILayout.Width(toggleWidth), GUILayout.Height(labelHeight));
			GUIUtility.RotateAroundPivot(-90, pos);
			pos.x += toggleWidth + 3f;
		}
		GUILayout.EndHorizontal();

		for (int v = 0; v < enumLength; v++)
		{
			EditorGUILayout.BeginHorizontal(style);
			EditorGUILayout.LabelField(enumNames[v], style, GUILayout.Width(labelWidth));
			for (int h = enumLength - 1; h >= 0; h--)
			{
				if (v >= h)
				{
					GUILayout.Space(toggleWidth);
					continue;
				}

				conditions.conditions[v, h] = EditorGUILayout.Toggle(conditions.conditions[v, h], GUILayout.Width(toggleWidth));
			}
			EditorGUILayout.EndHorizontal();
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void SetEnumNames()
	{
		if(conditions == null) { return; }

		switch(conditions.stateType)
		{
			case StateType.Player:
				enumNames = Enum.GetNames(typeof(PlayerState)).ToArray();
				break;

			case StateType.Enemy:
				enumNames = Enum.GetNames(typeof(EnemyState)).ToArray();
				break;
		}

		if(conditions.conditions == null)
		{
			conditions.conditions = new bool[enumNames.Length, enumNames.Length];
			
		}
		
		enumLength = enumNames.Length;
	}
}