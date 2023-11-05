using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum StateType
{
	Enemy,
	Player
}

[Serializable]
public struct BooleanArray
{
	public bool[] array;
}

[CreateAssetMenu(fileName = "StateChangeConditions", menuName = "ScriptatbleObject/State/ChangeConditions")]
public class StateChangeConditions : ScriptableObject
{
	public StateType stateType;
	[SerializeField] public BooleanArray[] conditions;

	public bool GetChangable(int current, int next)
	{
		int length = conditions.Length - 1;

		if(current == next) { return false; }					// 같은 State는 비허용
		if(current < 0 || current >= length) { return false; }	// current가 범위를 벗어난 경우 비허용(잘못된 입력)
		if(next < 0 || next >= length) { return false; }        // next가 범위를 벗어난 경우 비허용(잘못된 입력)

		return conditions[current].array[length - next];
	}
}

[CustomEditor(typeof(StateChangeConditions))]
public class StateChangeConditionsCustomEditor : Editor
{
	private int selectedEnumIndex = 0;
	private string[] enumNames;
	private int enumLength;

	private StateChangeConditions conditionDatas;
	private SerializedProperty conditions;
	private SerializedProperty stateType;

	private void OnEnable()
	{
		conditionDatas = target as StateChangeConditions;

		conditions = serializedObject.FindProperty("conditions");
		stateType = serializedObject.FindProperty("stateType");

		SetEnumNames();
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (conditions == null) { return; }

		SetEnumNames();

		var style = new GUIStyle();
		style.alignment = TextAnchor.MiddleRight;
		style.fixedHeight = 0;
		style.margin = new RectOffset(0, 0,0 ,0);
		style.normal.textColor = Color.cyan;

		float toggleWidth = 20f;
		float labelWidth = 175f;
		float labelHeight = 250f;
		EditorGUILayout.Space(50);
		EditorGUILayout.LabelField("세로 : 현재 상태");
		EditorGUILayout.LabelField("가로 : 다음 상태");

		serializedObject.Update();

		GUILayout.BeginHorizontal();

		GUILayout.Space(labelWidth * 0.25f);
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

		GUILayout.Space(labelWidth * -0.75f);

		GUI.color = Color.white;
		for (int v = 0; v < enumLength; v++)
		{
			SerializedProperty row = conditions.GetArrayElementAtIndex(v).FindPropertyRelative("array");

			EditorGUILayout.BeginHorizontal(style);
			EditorGUILayout.LabelField(enumNames[v], style, GUILayout.Width(labelWidth));
			for (int h = enumLength - 1; h >= 0; h--)
			{
				if (v == h)
				{
					GUILayout.Space(toggleWidth);
					continue;
				}

				SerializedProperty element = row.GetArrayElementAtIndex(h);

				EditorGUILayout.PropertyField(element, new GUIContent(""), GUILayout.Width(toggleWidth));
				serializedObject.ApplyModifiedProperties();
			}
			EditorGUILayout.EndHorizontal();
		}

		
	}

	private void SetEnumNames()
	{
		if(conditionDatas == null) { return; }

		switch(conditionDatas.stateType)
		{
			case StateType.Player:
				enumNames = Enum.GetNames(typeof(PlayerState)).ToArray();
				break;

			case StateType.Enemy:
				enumNames = Enum.GetNames(typeof(EnemyState)).ToArray();
				break;
		}

		if (conditionDatas.conditions == null || conditionDatas.conditions.Length != enumNames.Length)
		{
			conditionDatas.conditions = new BooleanArray[enumNames.Length];
			for(int i = 0; i < conditionDatas.conditions.Length; i++)
			{
				conditionDatas.conditions[i].array = new bool[enumNames.Length];
			}
			conditions = serializedObject.FindProperty("conditions");
			serializedObject.ApplyModifiedProperties();

			/*for (int i = 0; i < conditions.arraySize; i++)
			{
				var curIndex = conditions.GetArrayElementAtIndex(i);
				if (curIndex == null) { continue; }

				SerializedProperty row = conditions.GetArrayElementAtIndex(i).FindPropertyRelative("array");

				for (int j = 0; j < enumNames.Length; j++)
				{
					SerializedProperty element = row.GetArrayElementAtIndex(j);
					element.boolValue = true;
				}
			}*/
			
		}

		enumLength = enumNames.Length;
	}
}