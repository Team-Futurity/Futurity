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
	//#설명#	윈도우 총괄 메니저로 Window를 열고 닫는것은 물론
	[Header ("윈도우 시스템 총괄 메니저")]
	[Space(15)]

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

	private void Start()
	{
		topCanvasTransform = FindTopCanvas().transform;

		SceneManager.sceneLoaded += OnSceneLoaded;
		SelectButton(0);
	}

	private void OnDisable()
	{
		//#설명#	각 액션의 이벤트 핸들러를 제거합니다.


		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();
		selectAction.action.performed -= _ => ClickCurrentButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
		selectAction.action.Disable();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//#설명#	Scene이 로드될 때마다 topCanvasTransform을 다시 설정하기 위해 FindTopCanvas를 호출한다.


		topCanvasTransform = FindTopCanvas().transform;
	}

	private GameObject FindTopCanvas()
	{
		//#설명#	가장 상위 Canvas를 찾아서 topCanvasTransform에 할당함


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

	public void SetWindow(GameObject window)
	{
		windows.Add(window);
	}

	public void ClearWindow()
	{
		windows.Clear();
	}

	public GameObject HpBarOpen(GameObject hpBarObject)
	{
		//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수


		GameObject newWindow = Instantiate(hpBarObject, topCanvasTransform);

		return newWindow;
	}

	#region UIWindowOpen&Close
	public GameObject WindowOpen(GameObject openUiWindowObject, Transform WindowParent, bool isDeActive, Vector2 windowPosition, Vector2 windowScale)
	{
		//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수


		GameObject newWindow = Instantiate(openUiWindowObject, WindowParent);
		WindowController windowController = newWindow.GetComponent<WindowController>();
		if (!newWindow.CompareTag("UIWindow"))
		{
			newWindow.tag = "UIWindow";
		}
		RectTransform rectTransform = newWindow.GetComponent<RectTransform>();
		rectTransform.localPosition = windowPosition;
		rectTransform.localScale = windowScale;
		windows.Add(newWindow);
		SetButtons(windowController.GetButtons());


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

	public GameObject WindowTopOpen(GameObject openWindowObject, bool isDeActive, Vector2 windowPosition, Vector2 windowScale)
	{
		//#설명#	UI 창을 인스턴스화하되, 부모를 가장 상위 Canvas로 설정한다

		FDebug.Log($"topCanvasTransform {topCanvasTransform.name}");

		GameObject newWindow = WindowOpen(openWindowObject, topCanvasTransform, isDeActive, windowPosition, windowScale);

		return newWindow;
	}
	public void WindowClose(GameObject closeUiWindowObject)
	{
		//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수

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

	public void WindowChildAllClose(Transform parentTransform)
	{
		//#설명#	모든 자식 UI창을 닫는 함수


		foreach (Transform child in parentTransform)
		{
			if (child.CompareTag("UIWindow"))
				child.gameObject.GetComponent<WindowController>().WindowClose();
		}
	}

	public void WindowsClearner()
	{
		//#설명#	windows의 할당된 모든 window를 지우는 함수
		
	}
	#endregion


	#region SelectButton
	public void SetButtons(List<Button> buttons)
	{
		//#설명#	각 버튼값을 할당하며, 버튼 번호를 0으로 돌린다.
		this.buttons = buttons;

		SelectButton(0);
	}

	public void SetInputActionReference(InputActionReference leftAction, InputActionReference rightAction, InputActionReference selectAction)
	{
		//#설명#	각 액션에 대한 이벤트 핸들러를 등록합니다.
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
	public void EnableActionReference()
	{
		//#설명#	각 액션에 대한 이벤트 핸들러를 할당합니다.
		DisableActionReference();

		leftAction.action.performed += _ => SelectPreviousButton();
		rightAction.action.performed += _ => SelectNextButton();

		leftAction.action.Enable();
		rightAction.action.Enable();
	}
	public void DisableActionReference()
	{
		//#설명#	각 액션에 대한 이벤트 핸들러를 헤제합니다.
		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
	}

	private void SelectPreviousButton()
	{
		//#설명#	현재 선택된 버튼이 첫 번째 버튼이 아닌 경우 이전 버튼을 선택합니다.
		if (currentButtonIndex > 0)
		{
			SelectButton(currentButtonIndex - 1);
		}
	}

	private void SelectNextButton()
	{
		//#설명#	현재 선택된 버튼이 마지막 버튼이 아닌 경우 다음 버튼을 선택합니다.
		if (currentButtonIndex < buttons.Count - 1)
		{
			SelectButton(currentButtonIndex + 1);
		}
	}

	private void ClickCurrentButton()
	{
		//#설명#	현재 선택된 버튼을 클릭합니다.
		if (buttons.Count != 0)
		{
			Button currentButton = buttons[currentButtonIndex];
			ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		} else
		{
			FDebug.Log($"할당된 버튼이 없습니다. buttons.Count : {buttons.Count}");
		}
	}

	private void SelectButton(int index)
	{
		//#설명#	주어진 인덱스의 버튼을 선택합니다.
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


