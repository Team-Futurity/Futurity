using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SerializableDictionary<,>), true)]
public class SerializableCustomEditor : Editor
{
	// On
	protected void OnEnable()
	{
		
	}

	// Off
	protected void OnDisable()
	{
		
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}
