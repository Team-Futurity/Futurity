using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

	private DialogData nextDialogData;
	
	#region Dialog Events
	
	[HideInInspector]
	public UnityEvent OnStarted;

	[HideInInspector]
	public UnityEvent<DialogDataGroup> OnShow;

	[HideInInspector]
	public UnityEvent OnEnded;
	
	#endregion
	
	// SetDialogActive�� ���ؼ� Dialog System�� ų �� �ִ�.
	
	// InitDialogSystem(code)�� ���ؼ� Data�� �̸� �����Ѵ�.
	// PlayDialogSystem() �� ���ؼ� Dialog�� Play�Ѵ�.

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetDialogData("TEST");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlayDialog();
		}
	}

	public void SetDialogData(string code)
	{
		ChangeState(DialogSystemState.INIT);

		currentDialogData.Init();
		
		gameObject.SetActive(true);
		
		// Dialog�� ����Ǿ��ٸ�, Next Dialog�� �̵�
		DialogText.OnEnd.AddListener(GetNextDialog);
		
		OnStarted?.Invoke();
	}
	
	public void PlayDialog()
	{
		if (currentState == DialogSystemState.NONE)
		{
			return;
		}
		
		ChangeState(DialogSystemState.PRINTING);

		var (_,dialogData) = currentDialogData.GetCurrentData();
		
		// Dialog Text
		DialogText.Show(dialogData.descripton);
		
		OnShow?.Invoke(dialogData);
	}

	public void GetNextDialog()
	{
		ChangeState(DialogSystemState.PRINTING_END);
		
		currentDialogData.NextDialog();
	}

	private void CloseDialog()
	{
		ChangeState(DialogSystemState.NONE);
		
		gameObject.SetActive(false);
	}

	#region Dialog Feature

	public void EnterNextInteraction()
	{
		if (currentState == DialogSystemState.PRINTING_END)
		{
			PlayDialog();
			return;
		}
		
		DialogText.Pass();
	}

	public void OnDialogSkip()
	{
		
	}

	public void StopDialog()
	{
		DialogText.Stop();
	}
	
	#endregion

	private void ChangeState(DialogSystemState state)
	{
		currentState = state;
	}
}
