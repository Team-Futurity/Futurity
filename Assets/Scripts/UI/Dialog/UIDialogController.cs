using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct DialogData
{
	public string name;
	public string descripton;
}

public partial class UIDialogController : MonoBehaviour, IControllerMethod
{
	[field: Header("게임 실행 중 Type 변경을 권장하지 않음")]
	[field: SerializeField] 
	public UIDialogType DialogType { get; private set; }
	
	[field: Space(10)]
	
	[field: SerializeField] 
	public UIDialogText DialogText { get; private set; }

	private bool isActive;

	public UnityEvent onStart;
	public UnityEvent onEnd;
	public UnityEvent onChangeDialog;
	
	public UnityEvent<DialogData> onShow;
	private DialogData testDialog;

	private void Awake()
	{
		isActive = false;

		testDialog.name = "테스트";
		testDialog.descripton = "임시로 텍스트를 작성하고 있습니다...";
		
		DataSetUp(DialogType);
	}
	
	// 테스트용 코드
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Show();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Pass();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			DialogText.Stop();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			DialogText.Restart();
		}
	}

	#region IControllerMethod
	
	void IControllerMethod.Active()
	{
		
	}

	void IControllerMethod.Run()
	{
		
	}

	void IControllerMethod.Stop()
	{
		
	}
	
	#endregion

	public void Show()
	{
		onShow?.Invoke(testDialog);
		
		DialogText.Show(testDialog.descripton);
	}
	
	public void Skip()
	{
		
	}

	public void Pass()
	{
		DialogText.Pass();
	}
}
