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

	// Perform Instance�� ������ش�.
	public void SetData(UIPerformBoardHandler handler, UIDialogController controller)
	{
		this.performHandler = handler;
		this.controller = controller;
	}

	public void Play()
	{
		if (performHandler == null)
		{
			FDebug.Log($"Perform Handler�� ����ؾ� ��", GetType());
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
				// Start Event�� ���� ���
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

						// perform�� ���۵Ǳ� ���� �����ϰ� ����Ǹ� �����ش�.
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
			
			// Event Group ����
			for (int i = 0; i < 2; ++i)
			{
				if (!performHandler.HasGroup(i))
				{
					performHandler.CreateGroup(i);
				}
			}

			// Event Type�� ���� ��ȭ
			switch (eventType)
			{
				case DialogDirectingSource.DialogEventType.START:
					performHandler.AddPerformBoard(0, source.board);
					break;
				
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_START:
					
					performHandler.AddPerformBoard(0, source.board);

					// ���� Dialog�� ����Ǿ��ٸ�, ���� Dialog�� �����ϱ�
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
					
					// ���� Dialog�� �����ϱ� -> PerformBoard ���� ��, 
					source.board.onEndedAction += () =>
					{
						controller.NextDialog();
						controller.PlayUsedPlayer();
					};
					
					break;
			}
			
			lastSource = source;
		}

		// ������ �����Ͱ� ����� ���İ� �ش� Dialog�� ��
		lastSource.board.onEndedAction += () =>
		{
			onEnded?.Invoke();
		};
	}
}