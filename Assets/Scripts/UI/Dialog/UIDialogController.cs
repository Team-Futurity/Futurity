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
	[field: Header("게임 실행 중 Type 변경을 권장하지 않음")]
	[field: SerializeField] 
	public UIDialogType DialogType { get; private set; }
	
	[Space(10)]
	
	[Header("현재 Dialog Data")]
	[SerializeField]
	private DialogData currentDialogData;
	
	[field: Space(10)]

	[field: Header("텍스트가 출력되는 오브젝트")]
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
	
	// SetDialogActive를 통해서 Dialog System을 킬 수 있다.
	
	// InitDialogSystem(code)를 통해서 Data를 미리 세팅한다.
	// PlayDialogSystem() 을 통해서 Dialog를 Play한다.

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
		
		// Dialog가 종료되었다면, Next Dialog로 이동
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
