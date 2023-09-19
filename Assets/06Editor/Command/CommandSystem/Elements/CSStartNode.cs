using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CSStartNode : CSBaseNode
{

	public override void Draw()
	{
		// ~Title Container~ //
		TextField commandName = CSElementUtility.CreateTextField("시작 노드");

		commandName.AddClasses(
				"ds-node__textfield",
				"ds-node__filename-textfield",
				"ds-node__textfield__hidden"
		);

		titleContainer.Insert(0, commandName);

		// ~Output Container~ //
		Port nextCommandsPort = this.CreatePort("시작 커맨드", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
		nextCommandsPort.userData = this;
		outputContainer.Add(nextCommandsPort);
	}

	#region Overrided
	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		evt.menu.AppendAction("Disconnect output Ports", actionEvent => DisconnectOutputPorts());

		base.BuildContextualMenu(evt);
	}
	#endregion
}
