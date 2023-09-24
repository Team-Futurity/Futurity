using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.IO;

public class CommandEditorWindow : EditorWindow
{
	private CommandGraphView graphView;
	private readonly string defaultFileName = "DefaultName";
	private static TextField fileNameTextField;
	private Button saveButton;
	private Button miniMapButton;

    [MenuItem("Window/Player Command/Player Command Graph")]
    public static void ShowAttackGraph()
    {
        GetWindow<CommandEditorWindow>("�÷��̾� Ŀ�ǵ� �׷���");
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
		graphView = new CommandGraphView(this);
		graphView.StretchToParentSize();

		rootVisualElement.Add(graphView);
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
		Button clearButton = CSElementUtility.CreateButton("�����", () => Clear());
		Button resetButton = CSElementUtility.CreateButton("�ʱ�ȭ", () => ResetGraph());
		Button loadButton = CSElementUtility.CreateButton("�ҷ�����", () => Load());

		saveButton = CSElementUtility.CreateButton("����", () => Save());
		miniMapButton = CSElementUtility.CreateButton("�̴ϸ�", () => ToggleMiniMap());
		
		fileNameTextField = CSElementUtility.CreateTextField(defaultFileName, "���ϸ� : ", callback =>
		{
			fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
		});


		toolbar.Add(fileNameTextField);
		toolbar.Add(saveButton);
		toolbar.Add(loadButton);
		toolbar.Add(clearButton);
		toolbar.Add(resetButton);
		toolbar.Add(miniMapButton);

		toolbar.AddStyleSheets("CommandSystem/CSToolbarStyles.uss");

		rootVisualElement.Add(toolbar);
	}
	#endregion

	#region Toolbar Actions
	private void Save()
	{
		if(string.IsNullOrEmpty(fileNameTextField.value))
		{
			EditorUtility.DisplayDialog(
				"��ȿ���� ���� ���ϸ�",
				"��ȿ�� ���ϸ��� ��������",
				"��!"
			);

			return;
		}

		CSIOUtility.Initialize(graphView, fileNameTextField.value);
		CSIOUtility.Save();
	}

	private void Clear()
	{
		graphView.ClearGraph();

		SetEnableSaving(true);

		graphView.startNode = graphView.CreateStartNode();
	}

	private void ResetGraph()
	{
		Clear();

		UpdateFileName(defaultFileName);		
	}

	private void Load()
	{
		string filePath = EditorUtility.OpenFilePanel("Ŀ�ǵ� �׷���", "Assets/Editor/Command/CommandSystem/Graphs", "asset");

		if(!string.IsNullOrEmpty(filePath))
		{
			Clear();

			CSIOUtility.Initialize(graphView, Path.GetFileName(filePath));
			CSIOUtility.Load();
		}
	}

	private void ToggleMiniMap()
	{
		graphView.ToggleMiniMap();

		miniMapButton.ToggleInClassList("cs-toolbar__button__selected");
	}
	#endregion

	#region Utility
	public void SetEnableSaving(bool isEnable)
	{
		saveButton.SetEnabled(isEnable);
	}

	public static void UpdateFileName(string newFileName)
	{
		fileNameTextField.value = newFileName;
	}
	#endregion
}