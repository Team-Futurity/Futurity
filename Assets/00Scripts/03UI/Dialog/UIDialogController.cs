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
		
		SetUp();
	}

	private void OnDisable()
	{
		dialogText.onEnded?.RemoveListener(UpdateNextDialog);
	}

	private void SetUp()
	{
		if (dialogText == null)
		{
			FDebug.Log($"{dialogText}가 존재하지 않습니다.", GetType());
			FDebug.Break();

			return;
		}
		
		dialogText.onEnded?.AddListener(UpdateNextDialog);
	}
	
	public void SetDialogData(DialogData data)
	{
		dialogDatas.Add(data);
		
		InitDialog();
	}
	
	public void Play()
	{
		if (dialogText.isRunning) { return; }

		dialogText.Show(currentDialogData.GetDialogDataGroup().descripton);
		onShow?.Invoke(currentDialogData.GetDialogDataGroup());
	}
	
	private void OpenBoard(bool isOn)
	{
		gameObject.SetActive(isOn);
	}
	
	private void InitDialog()
	{
		if (currentDialogData == null)
		{
			currentDialogData = dialogDatas[currentIndex];
			currentDialogData.Init();
		}

		OpenBoard(true);
		
		onStarted?.Invoke();
	}

	private void UpdateNextDialog()
	{
		var isEnd = currentDialogData.Next();

		if (isEnd)
		{
			currentIndex += 1;

			if (currentIndex >= dialogDatas.Count)
			{
				OpenBoard(false);

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