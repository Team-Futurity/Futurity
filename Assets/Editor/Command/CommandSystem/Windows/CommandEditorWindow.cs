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
        GetWindow<CommandEditorWindow>("플레이어 커맨드 그래프");
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
		Button clearButton = CSElementUtility.CreateButton("지우기", () => Clear());
		Button resetButton = CSElementUtility.CreateButton("초기화", () => ResetGraph());
		Button loadButton = CSElementUtility.CreateButton("불러오기", () => Load());

		saveButton = CSElementUtility.CreateButton("저장", () => Save());
		miniMapButton = CSElementUtility.CreateButton("미니맵", () => ToggleMiniMap());
		
		fileNameTextField = CSElementUtility.CreateTextField(defaultFileName, "파일명 : ", callback =>
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
				"유효하지 않은 파일명",
				"유효한 파일명을 지으세요",
				"넵!"
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
		string filePath = EditorUtility.OpenFilePanel("커맨드 그래프", "Assets/Editor/Command/CommandSystem/Graphs", "asset");

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