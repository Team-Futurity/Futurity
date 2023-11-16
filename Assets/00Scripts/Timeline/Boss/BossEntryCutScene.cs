using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class BossEntryCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private BossController boss;
	private Animator bossAnimator;
	[SerializeField] private UIDialogController dialogController;

	[Header("다이얼로그 데이터")] 
	[SerializeField] private DialogData dialogData;
	
	[Header("플레이어 이동값")] 
	[SerializeField] private Transform endPos;
	[SerializeField] private float moveTime = 1.5f;

	[Header("Sound")] 
	[SerializeField] private ParamRef paramRef;
	
	[Header("Event")] 
	[SerializeField] private UnityEvent endEvent;
	
	protected override void Init()
	{
		base.Init();
		
		bossAnimator = boss.GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		boss.ActivateBoss();
		
		chapterManager.PlayerController.playerData.status.updateHPEvent
			?.Invoke(230f, 230f);
		
		chapterManager.scripting.ResetEmotion();
		chapterManager.scripting.DisableAllNameObject();
		
		dialogController.SetDialogData(dialogData);
		dialogController.Play();
		
		endEvent?.Invoke();
	}

	public void BossEntry_PrintScripts()
	{
		StartScripting();
	}

	public void BossEntry_StartSkeleton()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}

	public void BossEntry_PlayHitAni()
	{
		bossAnimator.SetTrigger("Hit");
	}

	public void BossEntry_PlayStartAni()
	{
		bossAnimator.SetTrigger("StartProduction");
	}

	public void PlayBossBackGroundMusic()
	{
		AudioManager.Instance.SetParameterForBackgroundMusic(paramRef);
	}
	
	public void MovePlayer()
	{
		chapterManager.PlayerController.LerpToWorldPosition(endPos.position, moveTime);
	}
}
