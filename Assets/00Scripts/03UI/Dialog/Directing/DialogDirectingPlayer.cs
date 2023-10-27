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

		foreach (var source in directingSource)
		{
			var eventType = source.eventType;

			if (currentDialog == null || currentDialog != source.dialogSource)
			{
				// Start Event가 없을 경우
				if (!hasStart && hasEnd)
				{
					UIManager.Instance.OpenWindow(WindowList.DIALOG_NORMAL);
					controller.Play();
				}

				hasStart = hasEnd = false;
				currentDialog = source.dialogSource;
				
				if (eventType == DialogDirectingSource.DialogEventType.START)
				{
					hasStart = true;
					
					source.dialogSource.onInit?.AddListener(() =>
					{
						// Dialog Open Perform
						performHandler.OpenPerform(0);

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
					hasEnd = true;
					
					source.dialogSource.onEnded?.AddListener(() =>
					{
						performHandler.onEnded?.RemoveAllListeners();
						UIManager.Instance.CloseWindow(WindowList.DIALOG_NORMAL);

						// Dialog Open Perform
						performHandler.OpenPerform(1);
					});
				}
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
				
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_START:
					
					performHandler.AddPerformBoard(0, source.board);

					// 현재 Dialog가 종료되었다면, 다음 Dialog를 실행하기
					source.dialogSource.onEnded?.AddListener(() =>
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
					
					// 다음 Dialog를 실행하기 -> PerformBoard 종료 시, 
					source.board.onEndedAction += () =>
					{
						controller.NextDialog();
						controller.PlayUsedPlayer();
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