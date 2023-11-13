using UnityEngine;

public class ActiveCutScene : CutSceneBase
{
	[Header("추가 컴포넌트")] 
	[SerializeField] private RectTransform dialogWindow;
	private float originYPos;
	private const float MOVE_YPOS = -1000.0f;
	
	private Animator playerAnimator;
	private readonly int ACTIVE_ALPHA_KEY = Animator.StringToHash("AlphaTrigger");
	private readonly int ACTIVE_BETA_KEY = Animator.StringToHash("BetaTrigger");

	protected override void Init()
	{
		base.Init();
		playerAnimator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);

		if (dialogWindow == null)
		{
			return;
		}
		
		originYPos = dialogWindow.anchoredPosition.y;
		SetRectYPos(MOVE_YPOS);
	}

	protected override void DisableCutScene()
	{
		Time.timeScale = 1.0f;
		chapterManager.SetActiveMainUI(true);
		
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
