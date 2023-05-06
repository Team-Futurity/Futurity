using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class CommandEditorWindow : EditorWindow
{
    [MenuItem("Window/Player Command/Player Command Graph")]
    public static void ShowAttackGraph()
    {
        GetWindow<CommandEditorWindow>("Player Command Graph");
    }

	private void OnEnable()
	{
		AddGraphView();

		AddStyles();
	}

	#region Elements Addition
	private void AddGraphView()
	{
		CommandGraphView graphview = new CommandGraphView(this);
		graphview.StretchToParentSize();

		rootVisualElement.Add(graphview);
	}

	private void AddStyles()
	{
		rootVisualElement.AddStyleSheets(
			"CommandSystem/CommandVariables.uss"
		);
	}
	#endregion
}