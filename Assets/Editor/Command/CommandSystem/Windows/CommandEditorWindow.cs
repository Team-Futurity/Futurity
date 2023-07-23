using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class CommandEditorWindow : EditorWindow
{
	private readonly string defaultFileName = "DefaultName";
	private TextField fileNameTextField;
	private Button saveButton;

    [MenuItem("Window/Player Command/Player Command Graph")]
    public static void ShowAttackGraph()
    {
        GetWindow<CommandEditorWindow>("Player Command Graph");
    }

	private void OnEnable()
	{
		AddGraphView();
		AddToolBar();

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

	private void AddToolBar()
	{
		Toolbar toolbar = new Toolbar();

		fileNameTextField = CSElementUtility.CreateTextField(defaultFileName, "File Name : ", callback =>
		{
			fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
		});

		saveButton = CSElementUtility.CreateButton("Save");

		toolbar.Add(fileNameTextField);
		toolbar.Add(saveButton);

		toolbar.AddStyleSheets("CommandSystem/CSToolbarStyles.uss");

		rootVisualElement.Add(toolbar);
	}
	#endregion

	#region Utility
	public void SetEnableSaving(bool isEnable)
	{
		saveButton.SetEnabled(isEnable);
	}
	#endregion
}