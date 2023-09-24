using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawnData))]
public class EnemySpawnDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.LabelField("사용 방법", " ");
		EditorGUILayout.LabelField("각 배열의 원소에는 스폰", "할 적 숫자를 아래와 같은 순서로 저장합니다.");
		EditorGUILayout.LabelField("0. MeleeDefault", " ");
		EditorGUILayout.LabelField("1. RangedDefault", " ");
		EditorGUILayout.LabelField("2. MinimalDefault", " ");
	}
}
