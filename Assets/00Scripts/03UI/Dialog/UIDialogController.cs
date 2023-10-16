using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DialogSystemState
{
	NONE,

	INIT,
	NORMAL,
	PRINTING,
	PRINTING_END,

	MAX
}

public partial class UIDialogController : MonoBehaviour
{
	[field: Header("���� ���� �� Type ������ �������� ����")]
	[field: SerializeField]
	public UIDialogType DialogType { get; private set; }

	[Space(10)]
	[Header("���� Dialog Data")]
	[SerializeField]
	private DialogData currentDialogData;

	[field: Space(10)]
	[field: Header("�ؽ�Ʈ�� ��µǴ� ������Ʈ")]
	[field: SerializeField]
	public UIDialogText DialogText { get; private set; }

	public DialogSystemState currentState;

	#region Dialog Events

	[HideInInspector]
	public UnityEvent OnStarted;

	[HideInInspector]
	public UnityEvent<DialogDataGroup> OnShow;

	[HideInInspector]
	public UnityEvent OnEnded;

	public bool isNext = false;

	#endregion

	public void SetDialogData(DialogData data)
	{
		currentDialogData = data;
		SetDialogData("TEST");
	}

	public void SetDialogData(string code)
	{
		ChangeState(DialogSystemState.INIT);

		currentDialogData.Init();

		gameObject.SetActive(true);

		if (DialogType == UIDialogType.NORMAL)
		{
			DialogText.OnEnd.AddListener(GetNextDialog);
		}

		OnStarted?.Invoke();
	}

	public void PlayDialog()
	{
		if (currentState == DialogSystemState.NONE)
		{
			return;
		}

		ChangeState(DialogSystemState.PRINTING);

		var (_, dialogData) = currentDialogData.GetCurrentData();

		DialogText.Show(dialogData.descripton);

		OnShow?.Invoke(dialogData);
	}

	public void GetNextDialog()
	{
		ChangeState(DialogSystemState.PRINTING_END);
		currentDialogData.NextDialog();

		EnterNextInteraction();
	}

	public void CloseDialog()
	{
		ChangeState(DialogSystemState.NONE);
		gameObject.SetActive(false);

		if (DialogType == UIDialogType.NORMAL)
		{
			DialogText.OnEnd.RemoveListener(GetNextDialog);
		}

		OnEnded?.Invoke();
	}

	#region Dialog Feature

	public void EnterNextInteraction()
	{
		if (currentState == DialogSystemState.PRINTING_END)
		{
			if (currentDialogData.GetLastData())
			{
				CloseDialog();

				return;
			}

			PlayDialog();
		}
	}

	#endregion

	private void ChangeState(DialogSystemState state)
	{
		currentState = state;
	}
}