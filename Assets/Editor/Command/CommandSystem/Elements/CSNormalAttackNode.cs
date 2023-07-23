using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CSNormalAttackNode : CSNode
{
	public override void Initialize(CommandGraphView cgView, Vector2 position)
	{
		base.Initialize(cgView, position);

		CommandType = CSCommandType.NormalAttack;
	}

	public override void Draw()
	{
		base.Draw();

		RefreshExpandedState();
	}
}
