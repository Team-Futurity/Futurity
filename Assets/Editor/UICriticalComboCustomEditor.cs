using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UICriticalCombo))]
public class UICriticalComboCustomEditor : Editor
{
	public int debugComboCount = 0;

	public override void OnInspectorGUI()
	{
		UICriticalCombo combo = (UICriticalCombo)target;

		debugComboCount = EditorGUILayout.IntField("Combo Gauge", debugComboCount);

		if (GUILayout.Button("Number Active"))
		{
			if (debugComboCount > 999) { debugComboCount = 999; }
			if (debugComboCount < 0) { debugComboCount = 0; }

			EditorApplication.RepaintProjectWindow();
		}

		if(GUILayout.Button("Critical Number"))
		{

			EditorApplication.RepaintProjectWindow();
		}

		base.OnInspectorGUI();

	}
}
