using UnityEngine;

public class C1_A3_EntryCutScene : CutSceneBase
{
	[Space(6)]
	[Header("컴포넌트")]
	private PlayerController playerController;
	
	protected override void Init()
	{
		base.Init();
	}
	
	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		chapterManager.isCutScenePlay = false;
	}
	
	public void Area3_PrintScripts()
	{
		StartScripting();
	}
}
