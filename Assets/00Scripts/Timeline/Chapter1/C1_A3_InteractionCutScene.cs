using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class C1_A3_InteractionCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private List<SpawnerManager> spawnerManager;
	[SerializeField] private List<BoxCollider> interactionCol;

	private bool isCutScenePlayed = false;
	private int curSpawnerIndex = -1;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		if (isCutScenePlayed == true)
		{
			return;
		}
		
		isCutScenePlayed = true;
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
		spawnerManager[curSpawnerIndex].SpawnEnemy();
	}

	public void CheckCutScenePlayed(int index)
	{
		if (isCutScenePlayed == false)
		{
			TimelineManager.Instance.EnableCutScene(ECutSceneType.CHAPTER1_AREA3_SPAWN);
		}

		curSpawnerIndex = index;
		
		spawnerManager[curSpawnerIndex].SpawnEnemy();
	}
}
