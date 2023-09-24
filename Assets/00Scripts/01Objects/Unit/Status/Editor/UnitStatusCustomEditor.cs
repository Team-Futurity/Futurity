using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OriginStatus))]
public class OriginStatusCustomEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		OriginStatus statusData = (OriginStatus)target;
		if (GUILayout.Button("Auto Generator"))
		{
			statusData.AutoGenerator();
			EditorApplication.RepaintProjectWindow();
		}
	}
}

[CustomEditor(typeof(StatusManager))]
public class StatusManagerCustomEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		StatusManager manager = (StatusManager)target;

		if (GUILayout.Button("Refresh"))
		{
			manager.CopyOrigin();
			EditorApplication.RepaintProjectWindow();
		}

	}
}