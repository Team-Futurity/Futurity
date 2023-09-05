using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct DialogData
{
	public string talker_Eng;
	public string talker_Kor;
	public string descripton;
}

public partial class UIDialogController : MonoBehaviour, IControllerMethod
{
	[field: Header("���� ���� �� Type ������ �������� ����")]
	[field: SerializeField] 
	public UIDialogType DialogType { get; private set; }
	
	[field: Space(10)]
	
	[field: Header("�ؽ�Ʈ�� ��µǴ� ������Ʈ")]
	[field: SerializeField] 
	public UIDialogText DialogText { get; private set; }


	// Dialog System�� �۵� �Ǿ��� ��, �ߵ��ϴ� �̺�Ʈ
	public UnityEvent onStart;
	// Dialog System�� ���� �Ǿ��� ��, �ߵ��ϴ� �̺�Ʈ
	public UnityEvent onEnd;
	// Dialog System�� ���� �Ǿ��� ��, �ߵ��ϴ� �̺�Ʈ
	public UnityEvent onChangeDialog;
	
	// ���������� ���Ǵ� ���� �̺�Ʈ
	private UnityEvent<DialogData> onShow;

	private List<DialogData> dialogDataList;

	private DialogData currentDialog;
	private DialogData nextDialog;

	private int currentIndex = 0;

	[SerializeField]
	private GameObject imageObject;

	// ���� Dialog System�� ����ִ°�?
	private bool isActive;

	private void Awake()
	{
		isActive = false;
		
		if(imageObject == null)
		{
			FDebug.Log($"{imageObject.GetType()}�� �������� �ʽ��ϴ�.");
			FDebug.Break();
		}

		DataSetUp();
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
		currentIndex = 0;
		dialogDataList.Clear();

		((IControllerMethod)this).Run();
	}

	void IControllerMethod.Run()
	{
		isActive = true;
	}

	void IControllerMethod.Stop()
	{
		isActive = false;
	}
	
	#endregion

	public void SetDialogData(string code)
	{
		if(isActive)
		{
			FDebug.LogError($"[Dialog System] ���� �߿��� Dialog �����͸� ������ �� �����ϴ�.");
			
			return;
		}

		// Initialize
		currentIndex = 0;
		dialogDataList.Clear();

		// Addressable Resource Path : Get Code
		// dialogDataList = new List<DialogData>();

		// ���� ��� ��ȭ ����
		nextDialog = dialogDataList[currentIndex++];
		
		// ���� ��ȭ ����
		SetCurrentData();
	}

	public void StartDialog()
	{
		if(isActive)
		{
			FDebug.LogError($"[Dialog System] �̹� �ý����� �������Դϴ�.");

			return;
		}

		isActive = true;

		imageObject.SetActive(true);
	}

	public void Show()
	{
		// �� ��ȭ�� �����ִ� ���̹Ƿ�, ������ ������ �� ������ �̷������ �ȴ�.

		//onShow?.Invoke(testDialog);
		//DialogText.Show(testDialog.descripton);
	}

	public void Skip()
	{
		// Skipt�� �ش� Dialog�� �Ѿ��.

	}

	public void Pass()
	{
		// Pass�� �ؽ�Ʈ�� ���� �����Ѵ�.

		DialogText.Pass();
	}

	private void SetCurrentData()
	{
		currentDialog = nextDialog;
		nextDialog = dialogDataList[++currentIndex];
	}

	private void End()
	{
		// Dialog System�� ����ȴ�.

		onEnd?.Invoke();
	}
}
