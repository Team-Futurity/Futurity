using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Alpha : CutSceneBase
{
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		Time.timeScale = 0.0f;
	}

	public override void DisableCutScene()
	{
		Time.timeScale = 1.0f;
	}
}
