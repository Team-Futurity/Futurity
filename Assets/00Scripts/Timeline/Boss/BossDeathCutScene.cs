using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossDeathCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector cutScene;
	
	[Header("Skeleton Cut Scene")] 
	[SerializeField] private Transform skeletonParent;
	private Queue<SkeletonGraphic> skeletonQueue;
	
	protected override void Init()
	{
		skeletonQueue = new Queue<SkeletonGraphic>();
		
		for (int i = 0; i < skeletonParent.childCount; ++i)
		{
			skeletonQueue.Enqueue(skeletonParent.GetChild(i).GetComponent<SkeletonGraphic>());
			skeletonParent.GetChild(i).gameObject.SetActive(false);
		}
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		
	}

	public void BossDeath_StartSkeleton()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}
}
