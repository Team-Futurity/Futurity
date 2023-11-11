using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDialogController : MonoBehaviour
{
	[field: Header("게임 실행 중 Type 변경을 권장하지 않음"), SerializeField]
	public UIDialogType DialogType { get; private set; }

	[Space(10), Header("텍스트가 출력되는 오브젝트"), SerializeField]
	private UIDialogText dialogText;
	public UIDialogText DialogText => dialogText;

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
		dialogDatas = new List<DialogData>();
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

	public void RemoveDialogEventAll()
	{
		for (int i = 0; i < dialogDatas.Count; ++i)
		{
			dialogDatas[i].onInit.RemoveAllListeners();
			dialogDatas[i].onChanged.RemoveAllListeners();
			dialogDatas[i].onEnded.RemoveAllListeners();
		}
	}

	private void InitDialog()
	{
		currentDialogData = dialogDatas[currentIndex];

		currentDialogData.Init();

		if (!usedPass)
		{
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

	private DialogData LoadDialogData(string code)
	{
		return Addressables.LoadAssetAsync<DialogData>(code).WaitForCompletion();
	}
}