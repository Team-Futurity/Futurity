using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CSNode : Node
{
	public string CommandName { get; set; }
	public CSCommandType CommandType { get; set; }

	public virtual void Initialize(Vector2 position)
	{
		CommandName = "CommandName";
		CommandType = CSCommandType.NormalAttack;

		SetPosition(new Rect(position, Vector2.zero));

		mainContainer.AddToClassList("ds-node__main-container");
		extensionContainer.AddToClassList("ds-node__extension-container");
	}

	public virtual void Draw()
	{
		// Title Container
		TextField commandName = CSElementUtility.CreateTextField(CommandName);

		commandName.AddClasses(
				"ds-node__textfield",
				"ds-node__filename-textfield",
				"ds-node__textfield__hidden"
		);

		//titleContainer.Insert(0, commandName);
		titleContainer.Add(commandName);

		// Input Container
		Port inputPort = this.CreatePort("이전 커맨드", Orientation.Horizontal, Direction.Input, Port.Capacity.Single);

		inputContainer.Add(inputPort);

		// Extensions Container
		VisualElement customDataContainer = new VisualElement();

		customDataContainer.AddToClassList("ds-node__custom-data-container");

		Foldout textFoldout = CSElementUtility.CreateFoldout("Command Type");
		EnumField enumField = new EnumField(CommandType);

		enumField.AddClasses(
				"ds-node__textfield",
				"ds-node__quote-textfield"
		);

		textFoldout.Add(enumField);
		customDataContainer.Add(textFoldout);

		extensionContainer.Add(customDataContainer);

		// Output Container
		Port nextCommands = this.CreatePort("다음 커맨드", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
		outputContainer.Add(nextCommands);
	}
}
