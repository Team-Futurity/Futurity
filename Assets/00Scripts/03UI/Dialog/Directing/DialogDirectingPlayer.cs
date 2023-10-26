using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogDirectingPlayer : MonoBehaviour
{
	[SerializeField]
	private List<DialogDirectingSource> directingSource;

	private UIPerformBoardHandler performHandler;
	private UIDialogController controller;

	private DialogData currentDialog;

	private List<int> saveDialogID = new List<int>();

	#region Unity Events

	public UnityEvent onPlay;
	public UnityEvent onEnded;

	#endregion

	// Perform Instance를 등록해준다.
	public void SetData(UIPerformBoardHandler handler, UIDialogController controller)
	{
		this.performHandler = handler;
		this.controller = controller;
	}

	public void Play()
	{
		if (performHandler == null)
		{
			FDebug.Log($"Perform Handler를 등록해야 함", GetType());
		}

		onPlay?.Invoke();

		UpdateAction();
		controller.PlayUsedPlayer();
	}

	private void UpdateAction()
	{
		DialogDirectingSource lastSource = new DialogDirectingSource();

		foreach (var source in directingSource)
		{
			var eventType = source.eventType;
			var dialogSource = source.dialogSource;
			
			if (currentDialog == null || currentDialog != dialogSource)
			{
				currentDialog = dialogSource;

				bool beforeSave = saveDialogID.Contains(currentDialog.GetInstanceID());

				if (!beforeSave)
				{
					// Init 이벤트 등록 -> 끝나고 PlayUsedPlayer를 통해서 Event를 실행한다.
					currentDialog.onInit?.AddListener(() =>
					{
						performHandler.OpenPerform(0);
						
						// PerformHandler의 모든 Data가 종료된다면, controller를 Play 해준다.
						performHandler.onEnded?.AddListener(() =>
						{
							UIManager.Instance.OpenWindow(WindowList.DIALOG_NORMAL);
							controller.Play();
						});
					});

					currentDialog.onEnded?.AddListener(() =>
					{
						performHandler.onEnded?.RemoveAllListeners();
						performHandler.OpenPerform(1);
					});
				}
				
				saveDialogID.Add(currentDialog.GetInstanceID());
			}
			
			// Event Group 생성
			for (int i = 0; i < 2; ++i)
			{
				if (!performHandler.HasGroup(i))
				{
					performHandler.CreateGroup(i);
				}
			}

			// Event Type에 따른 변화
			switch (eventType)
			{
				case DialogDirectingSource.DialogEventType.START:
					performHandler.AddPerformBoard(0, source.board);
					break;
				
				// 변경 시점 -> End Event가 없다는 소리.
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_START:
					// 같이 등록을 해준다.
					performHandler.AddPerformBoard(0, source.board);

					// 해당 Dialog 종료 시, Next Dialog 실행
					dialogSource.onEnded?.AddListener(() =>
					{
						controller.NextDialog();
						controller.Play();
					});
					
					break;
				
				case DialogDirectingSource.DialogEventType.END:
					performHandler.AddPerformBoard(1, source.board);
					break;
				
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_END:
					performHandler.AddPerformBoard(1, source.board);
					
					source.board.onEndedAction += () =>
					{
						controller.NextDialog();
						controller.PlayUsedPlayer();
					};
					
					break;
			}
			
			lastSource = source;
		}

		lastSource.board.onEndedAction += () =>
		{
			onEnded?.Invoke();
		};
	}
}