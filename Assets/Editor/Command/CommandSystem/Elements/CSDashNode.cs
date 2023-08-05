using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CSDashNode : CSNode
{
	public override void Initialize(string nodeName, CommandGraphView cgView, Vector2 position)
	{
		base.Initialize(nodeName, cgView, position);

		CommandType = CSCommandType.Dash;
	}

	public override void Draw()
	{
		base.Draw();

		RefreshExpandedState();
	}
}
