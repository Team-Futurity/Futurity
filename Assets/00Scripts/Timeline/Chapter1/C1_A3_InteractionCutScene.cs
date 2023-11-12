using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class C1_A3_InteractionCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private List<BoxCollider> interactionCol;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		interactionCol.ForEach(x => x.enabled = false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
	}

	public void Area3_PrintText()
	{
		StartScripting();
	}

	public void SpawnEnemy()
	{
		spawnerManager.SpawnEnemy();
	}
}
