/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static PlayerController;

[CustomEditor(typeof(EffectDatas))]
public class EffectSOCustomEditor : Editor
{
	private SerializedProperty effectDatasProperty;

	private void OnEnable()
	{
		effectDatasProperty = serializedObject.FindProperty("effectDatas");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(effectDatasProperty);

		if (effectDatasProperty.isArray && effectDatasProperty.arraySize > 0)
		{
			for (int i = 0; i < effectDatasProperty.arraySize; i++)
			{
				SerializedProperty effectDataProperty = effectDatasProperty.GetArrayElementAtIndex(i);

				SerializedProperty typeProperty = effectDataProperty.FindPropertyRelative("type");
				EffectType type = (EffectType)typeProperty.enumValueIndex;

				EditorGUILayout.PropertyField(typeProperty);

				switch (type)
				{
					case EffectType.Attack:
						SerializedProperty enumData1Property = effectDataProperty.FindPropertyRelative("enumData1");
						EditorGUILayout.PropertyField(enumData1Property);
						break;
					case EffectType.None:
						break;
						// Add more cases for other EffectType values and their respective Enum properties
				}
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}
}
*/