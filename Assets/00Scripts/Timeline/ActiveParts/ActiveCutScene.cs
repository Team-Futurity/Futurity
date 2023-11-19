using FMODUnity;
using UnityEngine;

public class ActiveCutScene : CutSceneBase
{
	[Header("추가 컴포넌트")] 
	[SerializeField] private RectTransform dialogWindow;
	[SerializeField] private EffectTimer effectTimer;
	private float originYPos;
	private const float MOVE_YPOS = -1000.0f;
	
	private Animator playerAnimator;
	private readonly int ACTIVE_ALPHA_KEY = Animator.StringToHash("AlphaTrigger");
	private readonly int ACTIVE_BETA_KEY = Animator.StringToHash("BetaTrigger");

	[Header("사운드")]
	[SerializeField] private FMODUnity.EventReference activeCutsceneSound;
	[SerializeField] private Transform playerPosition;

	protected override void Init()
	{
		base.Init();
		playerAnimator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		effectTimer.StartInitUnScaldTime(cutSceneType);

		if (dialogWindow == null)
		{
			return;
		}
		
		originYPos = dialogWindow.anchoredPosition.y;
		SetRectYPos(MOVE_YPOS);
		var soundInstance = AudioManager.Instance.CreateInstance(activeCutsceneSound);
		RuntimeManager.AttachInstanceToGameObject(soundInstance, playerPosition);
		soundInstance.start();
	}

	protected override void DisableCutScene()
	{
		Time.timeScale = 1.0f;
		chapterManager.SetActiveMainUI(true);
		effectTimer.StopInitTimer();
		
		if (dialogWindow == null)
		{
			return;
		}
		
		SetRectYPos(originYPos);
	}

	public void TimeStop()
	{
		if (cutSceneType == ECutSceneType.ACTIVE_ALPHA)
		{
			playerAnimator.SetTrigger(ACTIVE_ALPHA_KEY);
		}
		else
		{
			playerAnimator.SetTrigger(ACTIVE_BETA_KEY);
		}
		
		Time.timeScale = 0.0f;
	}

	private void SetRectYPos(float yPos)
	{
		Vector2 anchoredPos = dialogWindow.anchoredPosition;
		anchoredPos.y = yPos;
		dialogWindow.anchoredPosition = anchoredPos;
	}
}
