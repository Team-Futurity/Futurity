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
	public UIDialogText DialogText => dialogText;

	private int currentIndex;

	[SerializeField]
	private List<DialogData> dialogDatas;
	private DialogData currentDialogData;

	#region Dialog Events

	[Space(15)]
	public UnityEvent onStarted;
	public UnityEvent onChanged;
	public UnityEvent onEnded;

	[HideInInspector] public UnityEvent<DialogDataGroup> onShow;

	#endregion

	private void Awake()
	{
		dialogDatas = new List<DialogData>();
		currentIndex = 0;
		
		InitDialog();
	}

	public void SetDialogData(List<DialogData> datas)
	{
		dialogDatas = datas;
	}

	public void SetDialogData(DialogData data)
	{
		dialogDatas.Add(data);
	}

	public void Play()
	{
		if (dialogText.isRunning) { return; }

		dialogText.Show(currentDialogData.GetDialogDataGroup().descripton);
		onShow?.Invoke(currentDialogData.GetDialogDataGroup());
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

	public void ClearDialog()
	{
		currentDialogData = null;
		currentIndex = 0;
		
	}

	private void OnDisable()
	{
		isFirstSetting = false;
	}

	private bool isFirstSetting = false;
	private void InitDialog()
	{
		if (isFirstSetting == false)
		{
			isFirstSetting = true;
			currentDialogData = dialogDatas[currentIndex];
			currentDialogData.Init();
			
			dialogText.onEnded?.AddListener(UpdateNextDialog);
		}

		OpenDialogBoard(true);
		onStarted?.Invoke();
	}

	private void OpenDialogBoard(bool isOn)
	{
		gameObject.SetActive(isOn);
	}

	private void CloseDialog()
	{
		OpenDialogBoard(false);
		RemoveDialogEventAll();
	}

	private void UpdateNextDialog()
	{
		var isEnd = currentDialogData.Next();

		if (isEnd)
		{
			currentIndex += 1;

			if (currentIndex >= dialogDatas.Count)
			{
				CloseDialog();

				onEnded?.Invoke();
				return;
			}

			currentDialogData = dialogDatas[currentIndex];
			onChanged?.Invoke();

			return;
		}

		Play();
	}
}