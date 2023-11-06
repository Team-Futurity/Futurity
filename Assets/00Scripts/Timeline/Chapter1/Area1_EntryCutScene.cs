using Spine.Unity;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Area1_EntryCutScene : CutSceneBase
{
	[Header("Component")]
	[SerializeField] private SpawnerManager spawnerManager;
	[SerializeField] private PlayableDirector entryCutScene;

	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex;

	[Header("Skeleton Cut Scene")] 
	[SerializeField] private List<SkeletonGraphic> skeletonList;
	private Queue<SkeletonGraphic> skeletonQueue;
	
	protected override void Init()
	{
		skeletonQueue = new Queue<SkeletonGraphic>();

		foreach (SkeletonGraphic skeleton in skeletonList)
		{
			skeletonQueue.Enqueue(skeleton); 
			skeleton.gameObject.SetActive(false);
		}
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActiveMainUI(false);
	}
	
	protected override void DisableCutScene()
	{
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		chapterManager.SetActiveMainUI(true);
		chapterManager.isCutScenePlay = false;
	}

	public void Area1_StartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(entryCutScene, skeletonQueue);
	}
	
	public void Area1_Scripting()
	{
		entryCutScene.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(entryCutScene);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
	
	public void RandingPlayer()
	{
		//playerAni.speed = 1.0f;
	}

	public void Area1_SpawnEnemy()
	{
		spawnerManager.SpawnEnemy();
	}
}
