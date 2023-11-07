using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Area1_EntryCutScene : CutSceneBase
{
	[Space(6)] 
	[Header("=========== 추가 컴포넌트===========")]
	[SerializeField] private SpawnerManager spawnerManager;

	protected override void Init()
	{
		base.Init();
		chapterManager.PlayerController.SetLandingAnimation();
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

	public void Area1_StartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}
	
	public void Area1_Scripting()
	{
		base.StartScripting();
	}
	
	public void RandingPlayer()
	{
		chapterManager.PlayerController.PlayLandingAnimation();
	}

	public void Area1_SpawnEnemy()
	{
		spawnerManager.SpawnEnemy();
	}
}
