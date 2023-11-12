using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C1_A3_ExitCutScene : CutSceneBase
{
	[Header("추가 Component")] 
	[SerializeField] private ExitAnimation exitAnimation;

	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		base.EnableCutScene();
	}

	protected override void DisableCutScene()
	{
		exitAnimation.DoorOpenWait(() => ChapterMoveController.Instance.MoveNextChapter());
	}

	public void Area3_Exit()
	{
		StartScripting();
	}
}
