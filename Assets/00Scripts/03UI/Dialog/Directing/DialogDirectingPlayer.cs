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
					// Init �̺�Ʈ ��� -> ������ PlayUsedPlayer�� ���ؼ� Event�� �����Ѵ�.
					currentDialog.onInit?.AddListener(() =>
					{
						performHandler.OpenPerform(0);
						
						// PerformHandler�� ��� Data�� ����ȴٸ�, controller�� Play ���ش�.
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
				
				// ���� ���� -> End Event�� ���ٴ� �Ҹ�.
				case DialogDirectingSource.DialogEventType.NEXT_CHANGE_START:
					// ���� ����� ���ش�.
					performHandler.AddPerformBoard(0, source.board);

					// �ش� Dialog ���� ��, Next Dialog ����
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