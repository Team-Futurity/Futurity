using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitStatus))]
public class UnitStatusCustomEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		UnitStatus statusData = (UnitStatus)target;
		if (GUILayout.Button("Auto Generator"))
		{
			statusData.AutoGenerator();
			EditorApplication.RepaintProjectWindow();
		}
	}
}
