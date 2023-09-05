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

public partial class UIDialogController : MonoBehaviour, IControllerMethod
{
	[field: Header("게임 실행 중 Type 변경을 권장하지 않음")]
	[field: SerializeField] 
	public UIDialogType DialogType { get; private set; }
	
	[field: Space(10)]
	
	[field: Header("텍스트가 출력되는 오브젝트")]
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

	private bool usedCallMethod = false;

	private void Awake()
	{
		isActive = false;
		
		if(imageObject == null)
		{
			FDebug.Log($"{imageObject.GetType()}이 존재하지 않습니다.");
			FDebug.Break();
		}

		// DialogText에서 End 조건을 확인한다.
		DialogText.OnEnd?.AddListener(ShowAfter);
	}
	
	// 테스트용 코드
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetDialogData("TEST");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ShowDialog();
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			PlayDialog();
		}

		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			OnNextDialog();
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
			FDebug.LogError($"[Dialog System] 실행 중에는 Dialog 데이터를 변경할 수 없습니다.");
			return;
		}

		currentIndex = 0;
		nextIndex = 0;
		isTextEnd = false;
		isPrinting = false;

		// dialogDataList.Clear();

		// Addressable Resource Path : Get Code
		// dialogDataList = new List<DialogData>();

		// 다음 출력 대화 세팅
		nextDialog = dialogDataList[currentIndex];
	}

	public void ShowDialog()
	{
		if(isActive)
		{
			FDebug.LogError($"[Dialog System] 이미 시스템이 실행 중입니다.");
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
			FDebug.Log($"[Dialog System] 현재 시스템이 실행 중이 아닙니다.");
			return;
		}

		if(isTextEnd)
		{
			FDebug.LogError("[Dialog System] 최초 실행 시에만 사용이 가능한 메서드입니다.");
			FDebug.Break();

			return;
		}

		if(isPrinting)
		{
			FDebug.LogError($"[Dialog System] 현재 출력 중입니다.");

			return;
		}

		isPrinting = true;

		// Feature에게 Dialog Data를 전달하기 위함.
		OnPlay?.Invoke(currentDialog);

		// Text를 보여주는 메서드
		DialogText.Show(currentDialog.descripton);

		// 시작과 동시에, 다음 Dialog를 설정한다.
		SetCurrentData();
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
		OnDialogEnd?.Invoke(currentDialog);
	}

	// DialogText에 AddListener로 담은 메서드
	private void ShowAfter(bool isOn)
	{
		if(isOn)
		{
			return;
		}

		isPrinting = isOn;
		isTextEnd = !isOn;
	}

	// Pass 키를 누를 경우에 진행이 된다.
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
