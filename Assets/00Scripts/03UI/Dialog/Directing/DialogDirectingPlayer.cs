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
		bool isFirst = true;

		foreach (var source in directingSource)
		{
			var eventType = source.eventType;

			if (currentDialog == null || currentDialog != source.dialogSource)
			{
				// Start Event�� ���� ���
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
					Debug.Log($"{source.dialogSource.name}�� Start Event ��� �Ϸ�.");

					hasStart = true;
					
					source.dialogSource.onInit?.AddListener(() =>
					{
						// Dialog Open Perform
						performHandler.OpenPerform(source.dialogSource.GetInstanceID());

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
					Debug.Log($"{source.dialogSource.name}�� End Event ��� �Ϸ�.");

					hasEnd = true;

					// ���� Dialog�� ���ᰡ �ȴٸ�
					source.dialogSource.onEnded?.AddListener(() =>
					{
						UIManager.Instance.CloseWindow(WindowList.DIALOG_NORMAL);
						
						// Event ����
						performHandler.onEnded?.RemoveAllListeners();

						// Dialog Open Perform
						performHandler.OpenPerform(source.dialogSource.GetInstanceID() + 1);
					});
				}
			}

			// Event Group ����
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

			// Event Type�� ���� ��ȭ
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

					// ������ Board��� �Ҹ��̹Ƿ�, �ش� board�� End Event�� �ɾ��ش�.
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

		// ������ �����Ͱ� ����� ���İ� �ش� Dialog�� ��
		lastSource.board.onEndedAction += () =>
		{
			onEnded?.Invoke();
		};
	}
}