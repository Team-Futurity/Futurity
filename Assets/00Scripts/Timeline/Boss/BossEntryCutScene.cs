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
	
	[Header("플레이어 이동값")] 
	[SerializeField] private Transform endPos;
	[SerializeField] private float moveTime = 1.5f;

	protected override void Init()
	{
		bossAnimator = boss.GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	public override void DisableCutScene()
	{
		// chapterManager.SetActivePlayerInput(true);
		// chapterManager.SetActiveMainUI(true);
		// boss.isActive = true;
		//
		// chapterManager.PlayerController.playerData.status.updateHPEvent
		// 	?.Invoke(230f, 230f);
		
		chapterManager.scripting.ResetEmotion();
		chapterManager.scripting.DisableAllNameObject();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		SceneLoader.Instance.LoadScene("TitleScene");
	}

	public void BossEntry_PrintScripts()
	{
		bossEntry.Pause();
		
		chapterManager.PauseCutSceneUntilScriptsEnd(bossEntry);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
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
		chapterManager.PlayerController.LerpToWorldPosition(endPos.position, moveTime);
	}
}
