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
	public void SetData(UIPerformBoardHandler handler)
	{
		performHandler = handler;
	}

	public void Play()
	{
		if (performHandler == null)
		{
			FDebug.Log($"Perform Handler를 등록해야 함", GetType());
		}

		onPlay?.Invoke();

		UpdateAction();
	}

	private void UpdateAction()
	{
		DialogDirectingSource lastSource = new DialogDirectingSource();

		foreach (var source in directingSource)
		{
			var dialogSource = source.dialogSource;
			var eventType = source.eventType;

			// Current Dialog가 비어있거나, 지금과 동일하지 않을 경우 새로 추가한다.
			if (currentDialog == null || currentDialog != dialogSource)
			{
				currentDialog = dialogSource;
				
				bool beforeSave = saveDialogID.Contains(currentDialog.GetInstanceID());

				if (!beforeSave)
				{
					currentDialog.onInit?.AddListener(() =>
					{
						performHandler.OpenPerform(0);
					});

					currentDialog.onEnded?.AddListener(() =>
					{
						performHandler.OpenPerform(1);
					});
				}
				
				saveDialogID.Add(currentDialog.GetInstanceID());
			}

			for (int i = 0; i < 2; ++i)
			{
				if (!performHandler.HasGroup(i))
				{
					performHandler.CreateGroup(i);
				}
			}

			switch (eventType)
			{
				case DialogDirectingSource.DialogEventType.START:
					performHandler.AddPerformBoard(0, source.board);
					break;

				case DialogDirectingSource.DialogEventType.END:
					performHandler.AddPerformBoard(1, source.board);
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