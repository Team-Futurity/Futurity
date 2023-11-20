using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class C1_A3_InteractionCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private List<BoxCollider> interactionCol;
	[SerializeField] private List<GameObject> interactionEffect;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		
		interactionCol.ForEach(x => x.enabled = false);
		interactionEffect.ForEach(x => x.SetActive(false));
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
