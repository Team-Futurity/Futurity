using UnityEngine;
using UnityEngine.UI;

public class BossEntryCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private BossController boss;
	[SerializeField] private Image fadeImage;
	private Animator bossAnimator;
	
	[Header("플레이어 이동값")] 
	[SerializeField] private Transform endPos;
	[SerializeField] private float moveTime = 1.5f;

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

		fadeImage.color = new Color(255f, 255f, 255f, 0);
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
		bossAnimator.SetTrigger("Type3");
	}
	
	public void MovePlayer()
	{
		chapterManager.PlayerController.LerpToWorldPosition(endPos.position, moveTime);
	}
}
