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
	public DialogData currentDialogData;
	
	[field: Space(10)]

	[field: Header("텍스트가 출력되는 오브젝트")]
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
	
	// SetDialogActive를 통해서 Dialog System을 킬 수 있다.
	
	// InitDialogSystem(code)를 통해서 Data를 미리 세팅한다.
	// PlayDialogSystem() 을 통해서 Dialog를 Play한다.

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

		// Current Dialog Data를 Code에 맞게 수정해야 함.
		currentDialogData.Init();
		
		OnStarted?.Invoke();
	}
	
	public void PlayDialog()
	{
		ChangeState(DialogSystemState.PRINTING);

		// Current Dioalog Data가 존재하지 않을 경우, Play가 불가능하다.
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
