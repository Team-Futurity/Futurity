using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossEntryCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector bossEntry;
	[SerializeField] private BossController boss;
	[SerializeField] private CinemachineVirtualCamera bossCamera;
	private TimelineManager manager;
	private Animator bossAnimator;

	[Header("스크립트 데이터")] 
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;
	
	[Header("플레이어 이동값")] 
	[SerializeField] private Transform endPos;
	[SerializeField] private float moveTime = 1.5f;

	protected override void Init()
	{
		manager = TimelineManager.Instance;
		bossAnimator = boss.GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		manager.SetActivePlayerInput(false);
		manager.SetActiveMainUI(false);
	}

	public override void DisableCutScene()
	{
		manager.SetActivePlayerInput(true);
		manager.SetActiveMainUI(true);
		boss.isActive = true;
		
		TimelineManager.Instance.PlayerController.playerData.status.updateHPEvent
			.Invoke(230f, 230f);
	}

	public void BossEntry_PrintScripts()
	{
		bossEntry.Pause();
		
		manager.PauseCutSceneUntilScriptsEnd(bossEntry, scriptsList, curScriptsIndex);
		manager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
		
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
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
		manager.PlayerController.LerpToWorldPosition(endPos.position, moveTime);
	}
}
