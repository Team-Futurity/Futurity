using System;
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
		bool hasStart = false;
		bool hasEnd = false;
		bool isFirst = true;

		foreach (var source in directingSource)
		{
			var eventType = source.eventType;

			if (currentDialog == null || currentDialog != source.dialogSource)
			{
				// Start Event가 없을 경우
				if (!hasStart && hasEnd && isFirst)
				{
					isFirst = false;

					UIManager.Instance.OpenWindow(WindowList.DIALOG_NORMAL);
					controller.Play();
				}

				hasStart = hasEnd = false;
				currentDialog = source.dialogSource;
				
				if (eventType == DialogDirectingSource.DialogEventType.START)
				{
					Debug.Log($"{source.dialogSource.name}에 Start Event 등록 완료.");

					hasStart = true;
					
					source.dialogSource.onInit?.AddListener(() =>
					{
						// Dialog Open Perform
						performHandler.OpenPerform(source.dialogSource.GetInstanceID());

						// perform이 시작되기 전에 진행하고 종료되면 열어준다.
						performHandler.onEnded?.AddListener(() =>
						{
							UIManager.Instance.OpenWindow(WindowList.DIALOG_NORMAL);
							controller.Play();
						});
					});
				}

				if (eventType == DialogDirectingSource.DialogEventType.END)
				{
					Debug.Log($"{source.dialogSource.name}에 End Event 등록 완료.");

					hasEnd = true;

					// 현재 Dialog가 종료가 된다면
					source.dialogSource.onEnded?.AddListener(() =>
					{
						UIManager.Instance.CloseWindow(WindowList.DIALOG_NORMAL);
						
						// Event 제거
						performHandler.onEnded?.RemoveAllListeners();

						// Dialog Open Perform
						performHandler.OpenPerform(source.dialogSource.GetInstanceID() + 1);
					});
				}
			}

			// Event Group 생성
			var id = currentDialog.GetInstanceID();

			for (int i = 0; i < 2; ++i)
			{
				// Normal : Before, +1 : End
				if (!performHandler.HasGroup(id + i))
				{
					performHandler.CreateGroup(id + i);

					Debug.Log($"{currentDialog.name} + Create Group + {id + i}");
				}
			}

			// Event Type에 따른 변화
			switch (eventType)
			{
				case DialogDirectingSource.DialogEventType.START:
					performHandler.AddPerformBoard(currentDialog.GetInstanceID(), source.board);
					break;
				
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_START:
					performHandler.AddPerformBoard(currentDialog.GetInstanceID(), source.board);

					source.dialogSource.onEnded?.AddListener(() =>
					{
						controller.NextDialog();
						controller.Play();
					});
					
					break;
				
				case DialogDirectingSource.DialogEventType.END:
					performHandler.AddPerformBoard(currentDialog.GetInstanceID() + 1, source.board);
					break;
				
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_END:
					performHandler.AddPerformBoard(currentDialog.GetInstanceID() + 1, source.board);

					// 마지막 Board라는 소리이므로, 해당 board의 End Event를 걸어준다.
					source.board.onEndedAction += () =>
					{
						UIManager.Instance.OpenWindow(WindowList.DIALOG_NORMAL);

						controller.NextDialog();
						controller.Play();
					};

					break;
			}
			
			lastSource = source;
		}

		// 마지막 데이터가 종료된 이후가 해당 Dialog의 끝
		lastSource.board.onEndedAction += () =>
		{
			onEnded?.Invoke();
		};
	}
}