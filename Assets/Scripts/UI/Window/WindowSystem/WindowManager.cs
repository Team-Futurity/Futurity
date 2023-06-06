using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class WindowManager : Singleton<WindowManager>
{
	//#설명#	 윈도우의 생성, 관리 및 파괴를 담당하며, UI 윈도우와 버튼을 조작하고 관리하는 클래스입니다.
	[Header ("윈도우 시스템 총괄 메니저")]
	[Space(15)]

	private List<ObjectPoolManager<WindowController>> poolingWindows = new List<ObjectPoolManager<WindowController>>();
	private Dictionary<string, ObjectPoolManager<WindowController>> windowPools;

	public List<GameObject> windows = new List<GameObject>();
	public List<Button> buttons;

	[SerializeField]
	private int currentButtonIndex;
	[SerializeField]
	private GameObject currentButton;

	private Transform topCanvasTransform;

	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;

	protected override void Awake()
	{
		base.Awake();

		topCanvasTransform = FindTopCanvas().transform;
		windowPools = new Dictionary<string, ObjectPoolManager<WindowController>>();

		SceneManager.sceneLoaded += OnSceneLoaded;
		SelectButton(0);
	}

	///<summary>
	/// 각 액션의 이벤트 핸들러를 제거합니다.
	///</summary>
	private void OnDisable()
	{
		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();
		selectAction.action.performed -= _ => ClickCurrentButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
		selectAction.action.Disable();
	}

	///<summary>
	/// Scene이 로드될 때마다 최상위 캔버스를 다시 찾아서 설정합니다.
	///</summary>
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		topCanvasTransform = FindTopCanvas().transform;
	}

	///<summary>
	/// 모든 캔버스 중에서 가장 상위의 캔버스를 찾아서 반환합니다.
	///</summary>
	private GameObject FindTopCanvas()
	{
		Canvas[] allCanvases = FindObjectsOfType<Canvas>();

		Canvas topCanvas = null;
		foreach (Canvas canvas in allCanvases)
		{
			if (topCanvas == null || canvas.sortingOrder > topCanvas.sortingOrder)
			{
				topCanvas = canvas;
			}
		}

		if (topCanvas != null)
		{
			FDebug.Log("Top Canvas: " + topCanvas.name);
			return topCanvas.gameObject;
		}
		else
		{
			FDebug.Log("No Canvas found.");
			return null;
		}
	}


	#region ObjectPooling


	private int ObjectPooling(GameObject poolingWindow, GameObject windowParent)
	{
		poolingWindows.Add(new ObjectPoolManager<WindowController>(poolingWindow, windowParent));
		int poolingNum = poolingWindows.Count - 1;

		WindowController obj = poolingWindows[poolingNum].ActiveObject();
		poolingWindows[poolingNum].DeactiveObject(obj);

		return poolingNum;
	}
	#endregion

	#region UIWindowOpen&Close

	///<summary>
	/// 새로운 UI 창을 생성하고 부모와 위치를 설정합니다.
	///</summary>
	public GameObject WindowOpen(GameObject openUiWindowObject, Transform WindowParent, bool isDeActive, Vector2 windowPosition, Vector2 windowScale)
	{
		GameObject newWindow = Instantiate(openUiWindowObject, WindowParent);
		WindowController windowController = newWindow.GetComponent<WindowController>();
		RectTransform rectTransform = newWindow.GetComponent<RectTransform>();

		rectTransform.localPosition = windowPosition;
		rectTransform.localScale = windowScale;
		SetButtons(windowController.GetButtons());

		if (!newWindow.CompareTag("UIWindow"))
		{
			newWindow.tag = "UIWindow";
		}


		windows.Add(newWindow);

		if (isDeActive)
		{
			int windowNum = windows.Count - 1;
			for (int i = 0; i < windowNum; i++)
			{
				windows[i].SetActive(false);
			}
		}

		windowController.EnabledWindow();

		return newWindow;
	}

	///<summary>
	/// 새로운 UI 창을 생성하되 부모를 가장 상위 Canvas로 설정합니다.
	///</summary>
	public GameObject WindowTopOpen(GameObject openWindowObject, bool isDeActive, Vector2 windowPosition, Vector2 windowScale)
	{
		FDebug.Log($"topCanvasTransform {topCanvasTransform}");

		GameObject newWindow = WindowOpen(openWindowObject, topCanvasTransform, isDeActive, windowPosition, windowScale);

		return newWindow;
	}

	///<summary>
	/// 플레이어가 사용하지 않는 즉, 보여지기만 하는 창을 생성하고 위치를 설정합니다.
	///</summary>
	public GameObject DontUsedWindowOpen(GameObject dontUsedWindow)
	{
		GameObject newWindow = Instantiate(dontUsedWindow, topCanvasTransform);

		return newWindow;
	}

	///<summary>
	/// 특정 UI 창을 닫습니다.
	///</summary>
	public void WindowClose(GameObject closeUiWindowObject)
	{

		windows.Remove(closeUiWindowObject);
		int windowNum = windows.Count - 1;


		if (windowNum >= 0 && windows[windowNum] != null)
		{
			FDebug.Log($"windows[windowNum]{windows[windowNum]}");


			windows[windowNum].SetActive(true);

			WindowController windowController = windows[windowNum].GetComponent<WindowController>();
			SetButtons(windowController.GetButtons());
			windowController.EnabledWindow();
		}

		Destroy(closeUiWindowObject);
	}

	///<summary>
	/// 특정 부모 하위의 모든 UI 창을 닫습니다.
	///</summary>
	public void WindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if (child.CompareTag("UIWindow"))
				child.gameObject.GetComponent<WindowController>().WindowClose();
		}
	}

	///<summary>
	/// windows의 할당된 모든 window를 지워버립니다.
	///</summary>
	public void WindowsClearner()
	{
		foreach(GameObject deleteWindow in windows)
		{
			Destroy(deleteWindow);
		}

		buttons.Clear();
		ClearWindow();
	}

	///<summary>
	/// 윈도우 리스트에 값을 추가합니다.
	///</summary>
	public void SetWindow(GameObject window)
	{
		windows.Add(window);
	}

	///<summary>
	/// 윈도우 리스트를 초기화합니다.
	///</summary>
	public void ClearWindow()
	{
		windows.Clear();
	}

	#endregion

	#region SelectButton


	///<summary>
	/// 각 버튼값을 할당하며, 버튼 번호를 0으로 돌립니다.
	///</summary>
	public void SetButtons(List<Button> buttons)
	{
		this.buttons = buttons;

		SelectButton(0);
	}

	///<summary>
	/// 각 액션에 대한 이벤트 핸들러를 등록합니다.
	///</summary>
	public void SetInputActionReference(InputActionReference leftAction, InputActionReference rightAction, InputActionReference selectAction)
	{
		leftAction.action.performed += _ => SelectPreviousButton();
		rightAction.action.performed += _ => SelectNextButton();
		selectAction.action.performed += _ => ClickCurrentButton();

		this.leftAction = leftAction;
		this.rightAction = rightAction;
		this.selectAction = selectAction;

		leftAction.action.Enable();
		rightAction.action.Enable();
		selectAction.action.Enable();
	}

	///<summary>
	/// UI의 Button 선택 이벤트를 할당합니다.
	///</summary>
	public void EnableActionReference()
	{
		DisableActionReference();

		leftAction.action.performed += _ => SelectPreviousButton();
		rightAction.action.performed += _ => SelectNextButton();

		leftAction.action.Enable();
		rightAction.action.Enable();
	}
	///<summary>
	/// UI의 Button 선택 이벤트를 헤제합니다.
	///</summary>
	public void DisableActionReference()
	{
		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
	}

	///<summary>
	/// 현재 선택된 버튼이 첫 번째 버튼이 아닌 경우 이전 버튼을 선택합니다.
	///</summary>
	private void SelectPreviousButton()
	{
		if (currentButtonIndex > 0)
		{
			SelectButton(currentButtonIndex - 1);
		}
	}

	///<summary>
	/// 현재 선택된 버튼이 마지막 버튼이 아닌 경우 다음 버튼을 선택합니다.
	///</summary>
	private void SelectNextButton()
	{
		if (currentButtonIndex < buttons.Count - 1)
		{
			SelectButton(currentButtonIndex + 1);
		}
	}

	///<summary>
	/// 현재 선택된 버튼을 클릭합니다.
	///</summary>
	private void ClickCurrentButton()
	{
		if (buttons.Count != 0)
		{
			Button currentButton = buttons[currentButtonIndex];
			ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		} else
		{
			FDebug.Log($"할당된 버튼이 없습니다. buttons.Count : {buttons.Count}");
		}
	}

	///<summary>
	/// 주어진 인덱스의 버튼을 선택합니다.
	///</summary>
	private void SelectButton(int index)
	{
		currentButtonIndex = index;
		if (buttons != null && buttons.Count > 0)
		{
			if (buttons[currentButtonIndex] != null)
			{
				currentButton = buttons[currentButtonIndex].gameObject;
				EventSystem.current.SetSelectedGameObject(currentButton);
			}
		}
	}
	#endregion
} 


