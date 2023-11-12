using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionCutScene : CutSceneBase
{
	[Header("추가 Component")]
	[SerializeField] private List<int> firstName;
	private int enableIndex;

	[Header("종료 이벤트")]
	[SerializeField] private UnityEvent endEvent;

	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
		chapterManager.scripting.EnableNameText(firstName[enableIndex]);
		chapterManager.scripting.EnableStandingImg("SARI");
	}

	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		
		chapterManager.scripting.DisableAllNameObject();
		chapterManager.scripting.ResetEmotion();
		
		endEvent?.Invoke();
	}
	
	public void SubInteractionScripting()
	{
		StartScripting(enableIndex);
	}

	public void SetIndex(int index)
	{
		enableIndex = index;
	}
	public void MoveStage() => ChapterMoveController.Instance.MoveNextChapter();
}
