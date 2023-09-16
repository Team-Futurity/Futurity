using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialogData
{
	public string talker_Eng;
	public string talker_Kor;
	public string descripton;
}

public partial class UIDialogController : MonoBehaviour, IControlCommand
{
	[field: Header("���� ���� �� Type ������ �������� ����")]
	[field: SerializeField] 
	public UIDialogType DialogType { get; private set; }
	
	[field: Space(10)]
	
	[field: Header("�ؽ�Ʈ�� ��µǴ� ������Ʈ")]
	[field: SerializeField] 
	public UIDialogText DialogText { get; private set; }

	// --- Unity Events
	[HideInInspector]
	public UnityEvent<DialogData> OnShow;

	[HideInInspector]
	public UnityEvent<DialogData> OnPlay;

	[HideInInspector]
	public UnityEvent<DialogData> OnTextEnd;

	[HideInInspector]
	public UnityEvent<DialogData> OnDialogEnd;
	// 

	[SerializeField]
	private List<DialogData> dialogDataList;

	private DialogData currentDialog;
	private DialogData nextDialog;

	private int currentIndex = 0;
	private int nextIndex = 0;

	[SerializeField]
	private GameObject imageObject;

	private bool isActive;
	private bool isTextEnd;
	private bool isPrinting;

	private void Awake()
	{
		isActive = false;
		
		if(imageObject == null)
		{
			FDebug.Log($"{imageObject.GetType()}�� �������� �ʽ��ϴ�.");
			FDebug.Break();
		}

		// DialogText���� End ������ Ȯ���Ѵ�.
		DialogText.OnEnd?.AddListener(ShowAfter);
	}

	#region IControlCommand

	void IControlCommand.Init()
	{
		currentIndex = 0;
		dialogDataList.Clear();

		((IControlCommand)this).Run();
	}

	void IControlCommand.Run()
	{
		isActive = true;
	}

	void IControlCommand.Stop()
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

		currentIndex = 0;
		nextIndex = 0;
		isTextEnd = false;
		isPrinting = false;
		currentDialog.talker_Kor = currentDialog.talker_Eng = currentDialog.descripton = "";

		// dialogDataList.Clear();

		// Addressable Resource Path : Get Code
		// dialogDataList = new List<DialogData>();

		// ���� ��� ��ȭ ����
		nextDialog = dialogDataList[currentIndex];
	}

	public void ShowDialog()
	{
		if(isActive)
		{
			FDebug.LogError($"[Dialog System] �̹� �ý����� ���� ���Դϴ�.");
			return;
		}

		isActive = true;

		SetCurrentData();
		OnShow?.Invoke(currentDialog);

		imageObject.SetActive(true);
	}

	public void PlayDialog()
	{
		if(!isActive)
		{
			FDebug.Log($"[Dialog System] ���� �ý����� ���� ���� �ƴմϴ�.");
			return;
		}

		if(isTextEnd)
		{
			FDebug.LogError("[Dialog System] ���� ���� �ÿ��� ����� ������ �޼����Դϴ�.");
			FDebug.Break();

			return;
		}

		if(isPrinting)
		{
			FDebug.LogError($"[Dialog System] ���� ��� ���Դϴ�.");

			return;
		}

		isPrinting = true;

		// Feature���� Dialog Data�� �����ϱ� ����.
		OnPlay?.Invoke(currentDialog);

		// Text�� �����ִ� �޼���
		DialogText.Show(currentDialog.descripton);

		// ���۰� ���ÿ�, ���� Dialog�� �����Ѵ�.
		SetCurrentData();
	}

	public void OnPass()
	{
		if(!isActive)
		{
			return;
		}
		
		OnNextDialog();
	}

	public bool GetActive()
	{
		return isActive;
	}

	private void SetCurrentData()
	{
		currentDialog = nextDialog;
		currentIndex = nextIndex;

		nextIndex++;

		if (nextIndex < dialogDataList.Count)
		{
			nextDialog = dialogDataList[nextIndex];
		}
	}

	private void EndDialog()
	{
		((IControlCommand)this).Stop();

		imageObject.SetActive(false);

		DialogText.ClearText();

		OnDialogEnd?.Invoke(currentDialog);
	}

	// DialogText�� AddListener�� ���� �޼���
	private void ShowAfter(bool isOn)
	{
		if(isOn)
		{
			return;
		}

		isPrinting = isOn;
		isTextEnd = !isOn;
	}

	// Pass Ű�� ���� ��쿡 ������ �ȴ�.
	private void OnNextDialog()
	{
		if(!isTextEnd)
		{
			DialogText.Stop();
			DialogText.Pass();

			return;
		}

		isTextEnd = false;

		if(currentIndex >= dialogDataList.Count)
		{
			EndDialog();
			
			return;
		}
		
		PlayDialog();
	}
}

// Closed Action ����� �־�� ��.
