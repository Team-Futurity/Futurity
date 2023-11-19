using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
	[SerializeField] private EventReference bg;
	[SerializeField] private List<EventReference> cutSceneSound;
	private EventInstance soundInst;
	private int curSoundIndex;
	private int curPlayIndex;

	[Header("Event")] 
	[SerializeField] private UnityEvent endEvent;
	
	// Parameter Hash Key
	private readonly int BOSS_HIT_KEY = Animator.StringToHash("Hit");
	private readonly int BOSS_START_KEY = Animator.StringToHash("StartProduction");
	
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
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue, PlayCutSceneSound);
	}

	public void BossEntry_PlayHitAni() => bossAnimator.SetTrigger(BOSS_HIT_KEY);
	public void BossEntry_PlayStartAni() => bossAnimator.SetTrigger(BOSS_START_KEY);
	public void MovePlayer() => chapterManager.PlayerController.LerpToWorldPosition(endPos.position, moveTime);
	public void PlayBackGroundMusic() => AudioManager.Instance.RunBackgroundMusic(bg);

	public void StopSound()
	{
		soundInst.stop(STOP_MODE.IMMEDIATE);
		curSoundIndex = 0;
		curPlayIndex = 0;
	}

	private void PlayCutSceneSound()
	{
		switch (curPlayIndex)
		{
			case 2:
				soundInst = AudioManager.Instance.CreateInstance(cutSceneSound[curSoundIndex++]);
				soundInst.start();
				break;
			
			case 3:
				soundInst.stop(STOP_MODE.IMMEDIATE);
				soundInst = AudioManager.Instance.CreateInstance(cutSceneSound[curSoundIndex++]);
				soundInst.start();
				break;
			
			case 12:
				soundInst.stop(STOP_MODE.IMMEDIATE);
				soundInst = AudioManager.Instance.CreateInstance(cutSceneSound[curSoundIndex++]);
				soundInst.start();
				break;
			
			case 13:
				soundInst.stop(STOP_MODE.IMMEDIATE);
				soundInst = AudioManager.Instance.CreateInstance(cutSceneSound[curSoundIndex++]);
				soundInst.start();
				break;
			
			case 14:
				soundInst.stop(STOP_MODE.IMMEDIATE);
				soundInst = AudioManager.Instance.CreateInstance(cutSceneSound[curSoundIndex++]);
				soundInst.start();
				break;
		}

		curPlayIndex++;
	}
}
