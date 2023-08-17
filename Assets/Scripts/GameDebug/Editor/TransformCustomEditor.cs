using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class TransformCustomEditor : Editor
{
	private Editor m_editor;
	private SerializedProperty m_positionProperty;
	private SerializedProperty m_rotationProperty;
	private SerializedProperty m_scaleProperty;

	private void OnEnable()
	{
		if (serializedObject == null)
			return;

		m_positionProperty = serializedObject.FindProperty("m_LocalPosition");
		m_rotationProperty = serializedObject.FindProperty("m_LocalRotation");
		m_scaleProperty = serializedObject.FindProperty("m_LocalScale");
	}

	private void OnDisable()
	{
		if (serializedObject == null)
			return;

		m_positionProperty = null;
		m_rotationProperty = null;
		m_scaleProperty = null;
	}

	public override void OnInspectorGUI()
	{
		if (m_editor == null)
		{
			var type = typeof(Editor).Assembly.GetType("UnityEditor.TransformInspector");
			m_editor = CreateEditor(target, type);
		}

		m_editor.OnInspectorGUI();

		if (GUILayout.Button("Reset"))
		{
			m_positionProperty.vector3Value = Vector3.zero;
			m_rotationProperty.quaternionValue = Quaternion.identity;
			m_scaleProperty.vector3Value = Vector3.one;
		}

		serializedObject.ApplyModifiedProperties();
	}
}