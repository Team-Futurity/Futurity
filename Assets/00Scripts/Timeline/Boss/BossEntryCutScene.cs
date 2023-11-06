using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossEntryCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector bossEntry;
	[SerializeField] private BossController boss;
	private Animator bossAnimator;

	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;
	
	[Header("Skeleton Cut Scene")] 
	[SerializeField] private Transform skeletonParent;
	private Queue<SkeletonGraphic> skeletonQueue;
	
	[Header("플레이어 이동값")] 
	[SerializeField] private Transform endPos;
	[SerializeField] private float moveTime = 1.5f;

	protected override void Init()
	{
		bossAnimator = boss.GetComponentInChildren<Animator>();
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
		chapterManager.SetActiveMainUI(true);
		 boss.isActive = true;
		
		 chapterManager.PlayerController.playerData.status.updateHPEvent
		 	?.Invoke(230f, 230f);
		
		chapterManager.scripting.ResetEmotion();
		chapterManager.scripting.DisableAllNameObject();
	}

	public void BossEntry_PrintScripts()
	{
		bossEntry.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(bossEntry);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}

	public void BossEntry_StartSkeleton()
	{
		chapterManager.StartSkeletonCutScene(bossEntry, skeletonQueue);
	}

	public void BossEntry_PlayHitAni()
	{
		bossAnimator.SetTrigger("Hit");
	}

	public void BossEntry_PlayStartAni()
	{
		bossAnimator.SetTrigger("Type3");
	}
	
	public void MovePlayer()
	{
		chapterManager.PlayerController.LerpToWorldPosition(endPos.position, moveTime);
	}
}
