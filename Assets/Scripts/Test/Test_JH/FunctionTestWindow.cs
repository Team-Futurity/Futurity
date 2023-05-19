/*using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class FunctionTestWindow : EditorWindow
{
    [MenuItem("Window/FunctionTestWindow")]

    public static void ShowWindow()
    {
        GetWindow<FunctionTestWindow>("FunctionTestWindow");
    }

    private GameObject targetObject;
    //private UnityEvent testFunction;

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        targetObject = EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true) as GameObject;

        if (targetObject != null)
        {
            Debug.Log($"targetObject이 없습니다.");
        }

        if (GUILayout.Button("Run Function") && targetObject != null)
        {
            targetObject.GetComponent<EnemyBuffController>().Knockback(Vector2.right, 300f);
        }

        EditorGUILayout.EndVertical();
    }
}*/