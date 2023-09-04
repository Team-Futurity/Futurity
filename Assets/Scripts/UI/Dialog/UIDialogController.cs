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
	[field: Header("���� ���� �� Type ������ �������� ����")]
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

		testDialog.name = "�׽�Ʈ";
		testDialog.descripton = "�ӽ÷� �ؽ�Ʈ�� �ۼ��ϰ� �ֽ��ϴ�...";
		
		DataSetUp(DialogType);
	}
	
	// �׽�Ʈ�� �ڵ�
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
