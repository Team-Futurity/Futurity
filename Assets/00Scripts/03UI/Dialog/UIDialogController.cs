using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDialogController : MonoBehaviour
{
	[field: Header("���� ���� �� Type ������ �������� ����"), SerializeField]
	public UIDialogType DialogType { get; private set; }

	[Space(10), Header("�ؽ�Ʈ�� ��µǴ� ������Ʈ"), SerializeField]
	private UIDialogText dialogText;

	private int currentIndex;

	[SerializeField]
	private List<DialogData> dialogDatas;
	private DialogData currentDialogData;

	[SerializeField]
	private bool usedPass = false;
	
	#region Dialog Events

	[Space(15)]
	public UnityEvent onStarted;
	public UnityEvent onChanged;
	public UnityEvent onEnded;

	[HideInInspector] public UnityEvent<DialogDataGroup> onShow;

	#endregion
	
	private void Awake()
	{
		//dialogDatas = new List<DialogData>();
		currentIndex = 0;
	}

	public void SetDialogData(List<DialogData> datas)
	{
		dialogDatas = datas;
		InitDialog();
	}

	public void SetDialogData(DialogData data)
	{
		dialogDatas.Add(data);
		InitDialog();
	}

	public void SetDialogData(string code)
	{
		LoadDialogData(code);
		InitDialog();
	}

	public void PlayUsedPlayer()
	{
		InitDialog();
	}

	public void Play()
	{
		if (dialogText.isRunning) { return; }
		
		dialogText.Show(currentDialogData.GetDialogDataGroup().descripton);
		onShow?.Invoke(currentDialogData.GetDialogDataGroup());
	}

	public void Pass()
	{
		if (dialogText.isRunning)
		{
			dialogText.Pass();
		}
		else
		{
			UpdateNextDialog();
		}
	}

	public void SetDialogInitEvent(int index, UnityAction action)
	{
		if (index >= dialogDatas.Count) return;
		dialogDatas[index].onInit?.AddListener(action);
	}

	public void SetDialogChangedEvent(int index, UnityAction action)
	{
		if (index >= dialogDatas.Count) return;
		dialogDatas[index].onChanged?.AddListener(action);
	}

	public void SetDialogEndedEvent(int index, UnityAction action)
	{
		if (index >= dialogDatas.Count) return;
		dialogDatas[index].onEnded?.AddListener(action);
	}

	public void RemoveDialogEventAll()
	{
		for (int i = 0; i < dialogDatas.Count; ++i)
		{
			dialogDatas[i].onInit.RemoveAllListeners();
			dialogDatas[i].onChanged.RemoveAllListeners();
			dialogDatas[i].onEnded.RemoveAllListeners();
		}
	}
	
	public void NextDialog()
	{
		dialogText.onEnded?.RemoveAllListeners();
		
		// Next Index
		currentIndex += 1;

		// Index�� DialogDatas���� ���� ���
		if (currentIndex >= dialogDatas.Count)
		{
			CloseDialog();
			
			onEnded?.Invoke();
			return;
		}

		// Dialog Data�� ���Ƴ����.
		InitDialog();
		
		// Play�� �ʿ��ϴ�.
	}

	private void InitDialog()
	{
		currentDialogData = dialogDatas[currentIndex];
		currentDialogData.Init();

		if (!usedPass)
		{
			dialogText.onEnded?.AddListener(UpdateNextDialog);
		}

		onStarted?.Invoke();
	}
	
	private void CloseDialog()
	{
		RemoveDialogEventAll();
	}

	private void UpdateNextDialog()
	{
		var isEnd = currentDialogData.Next();

		if (isEnd)
		{
			return;
		}
		
		Play();
	}

	private DialogData LoadDialogData(string code)
	{
		return Addressables.LoadAssetAsync<DialogData>(code).WaitForCompletion();
	}
}