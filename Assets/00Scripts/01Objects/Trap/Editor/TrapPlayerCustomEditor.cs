using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(MonoBehaviour))]
// public class TrapPlayerCustomEditor : Editor
// {
// 	private string[] propertiesToHide;
// 	private bool hasHiddenProperties = false;
//  
// 	private void OnEnable()
// 	{
// 		var attrs = (HidePropertiesAttribute[])target.GetType().GetCustomAttributes(typeof(HidePropertiesAttribute), false);
// 		
// 		if (attrs[0].PropertyNames is not null)
// 		{
// 			propertiesToHide = attrs[0].PropertyNames;
// 			hasHiddenProperties = true;
// 		}
// 	}
//  
// 	public override void OnInspectorGUI()
// 	{
// 		if (hasHiddenProperties)
// 		{
// 			serializedObject.Update();
// 			EditorGUI.BeginChangeCheck();
// 			DrawPropertiesExcluding(serializedObject, propertiesToHide);
// 			if (EditorGUI.EndChangeCheck())
// 				serializedObject.ApplyModifiedProperties();
// 		}
// 		else
// 			base.OnInspectorGUI();
// 	}
// }
//
// [AttributeUsage(AttributeTargets.Class, Inherited = false)]
// public class HidePropertiesAttribute : Attribute
// {
// 	public string[] PropertyNames;
//
// 	public HidePropertiesAttribute(params string[] propertyNames)
// 	{
// 		PropertyNames = propertyNames;
// 	}
// }