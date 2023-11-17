using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawnData))]
public class CustomSpanwerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		EnemySpawnData spawnData = (EnemySpawnData)target;

		EditorGUILayout.LabelField("아래의 순서대로 몬스터 스폰값을 입력(배열 기본 길이 : 6).");
		EditorGUILayout.LabelField("1. Melee Default");
		EditorGUILayout.LabelField("2. Ranged Default");
		EditorGUILayout.LabelField("3. Minimal Default");
		EditorGUILayout.LabelField("4. Elite Default");
		EditorGUILayout.LabelField("5. D_BF");
		EditorGUILayout.LabelField("6. M_JF");
	}
}
