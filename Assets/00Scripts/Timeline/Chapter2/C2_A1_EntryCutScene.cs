using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class C2_A1_EntryCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private SpawnerManager spawnerManager;

	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		spawnerManager.SpawnEnemy();
		
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		chapterManager.SetActiveMainUI(true);
	}

	public void C2A1_Scripting()
	{
		StartScripting();
	}
}
