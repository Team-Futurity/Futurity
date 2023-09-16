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
	public DialogData currentDialogData;
	
	[field: Space(10)]

	[field: Header("�ؽ�Ʈ�� ��µǴ� ������Ʈ")]
	[field: SerializeField] 
	public UIDialogText DialogText { get; private set; }

	[SerializeField] private DialogSystemState currentState;

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
			InitDialog("TEST");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlayDialog();
		}
	}

	public void InitDialog(string code)
	{
		ChangeState(DialogSystemState.INIT);

		// Current Dialog Data�� Code�� �°� �����ؾ� ��.
		currentDialogData.Init();
		
		OnStarted?.Invoke();
	}
	
	public void PlayDialog()
	{
		ChangeState(DialogSystemState.PRINTING);

		// Current Dioalog Data�� �������� ���� ���, Play�� �Ұ����ϴ�.
		var (_,dialogData) = currentDialogData.GetCurrentData();
		DialogText.Show(dialogData.descripton);
		
		OnShow?.Invoke(dialogData);
	}

	// Dialog Open
	public void SetDialogActive(bool isOn)
	{
		gameObject.SetActive(isOn);
	}
	
	#region Dialog Feature

	public void OnTextPass()
	{
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
