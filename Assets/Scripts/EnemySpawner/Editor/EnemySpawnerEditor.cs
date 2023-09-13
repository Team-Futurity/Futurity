using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EnemySpawner generator = (EnemySpawner)target;
		
		if (GUILayout.Button("Generate Position"))
		{
			generator.InstantiatePosition();
		}

		if (GUILayout.Button("Remove All Position"))
		{
			generator.RemoveAllPosition();
		}
	}
}
