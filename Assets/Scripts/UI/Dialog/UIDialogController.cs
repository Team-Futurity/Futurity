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
	[field: Header("게임 실행 중 Type 변경을 권장하지 않음")]
	[field: SerializeField] 
	public UIDialogType DialogType { get; private set; }
	
	[field: Space(10)]
	
	[field: Header("텍스트가 출력되는 오브젝트")]
	[field: SerializeField] 
	public UIDialogText DialogText { get; private set; }


	// Dialog System이 작동 되었을 때, 발동하는 이벤트
	public UnityEvent onStart;
	// Dialog System이 종료 되었을 때, 발동하는 이벤트
	public UnityEvent onEnd;
	// Dialog System이 변경 되었을 때, 발동하는 이벤트
	public UnityEvent onChangeDialog;
	
	// 내부적으로 사용되는 변경 이벤트
	private UnityEvent<DialogData> onShow;

	private List<DialogData> dialogDataList;

	private DialogData currentDialog;
	private DialogData nextDialog;

	private int currentIndex = 0;

	[SerializeField]
	private GameObject imageObject;

	// 현재 Dialog System이 살아있는가?
	private bool isActive;

	private void Awake()
	{
		isActive = false;
		
		if(imageObject == null)
		{
			FDebug.Log($"{imageObject.GetType()}이 존재하지 않습니다.");
			FDebug.Break();
		}

		DataSetUp();
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

		// Initialize
		currentIndex = 0;
		dialogDataList.Clear();

		// Addressable Resource Path : Get Code
		// dialogDataList = new List<DialogData>();

		// 다음 출력 대화 세팅
		nextDialog = dialogDataList[currentIndex++];
		
		// 현재 대화 세팅
		SetCurrentData();
	}

	public void StartDialog()
	{
		if(isActive)
		{
			FDebug.LogError($"[Dialog System] 이미 시스템이 실행중입니다.");

			return;
		}

		isActive = true;

		imageObject.SetActive(true);
	}

	public void Show()
	{
		// 한 대화를 보여주는 곳이므로, 데이터 세팅이 이 곳에서 이루어지면 된다.

		//onShow?.Invoke(testDialog);
		//DialogText.Show(testDialog.descripton);
	}

	public void Skip()
	{
		// Skipt은 해당 Dialog를 넘어간다.

	}

	public void Pass()
	{
		// Pass는 텍스트를 전부 추출한다.

		DialogText.Pass();
	}

	private void SetCurrentData()
	{
		currentDialog = nextDialog;
		nextDialog = dialogDataList[++currentIndex];
	}

	private void End()
	{
		// Dialog System이 종료된다.

		onEnd?.Invoke();
	}
}
