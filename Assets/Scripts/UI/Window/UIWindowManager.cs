using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIWindowManager : Singleton<UIWindowManager>
{
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
			Debug.Log("Top Canvas: " + topCanvas.name);
			return topCanvas.gameObject;
		}
		else
		{
			Debug.Log("No Canvas found.");
			return null;
		}
	}

	public void SetWindow(GameObject window)
	{
		windows.Add(window);
	}



	#region UIWindowOpen&Close
	public GameObject UIWindowOpen(GameObject openUiWindowObject, Transform uiParent, Vector2 instancePosition, Vector2 windowScale)
	{
		//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수


		GameObject newUI = Instantiate(openUiWindowObject, uiParent);
		if (!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;
		rectTransform.localScale = windowScale;
		windows.Add(newUI);

		SetButtons(newUI.GetComponent<UIWindowController>().GetButtons());
		return newUI;
	}

	public GameObject UIWindowTopOpen(GameObject openUiWindowObject, Vector2 windowPosition, Vector2 windowScale)
	{
		//#설명#	UI 창을 인스턴스화하되, 부모를 가장 상위 Canvas로 설정한다


		GameObject newUI = Instantiate(openUiWindowObject, topCanvasTransform);
		if (!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = windowPosition;
		rectTransform.localScale = windowScale;
		windows.Add(newUI);

		SetButtons(newUI.GetComponent<UIWindowController>().GetButtons());

		return newUI;
	}
	public void UIWindowClose(GameObject closeUiWindowObject)
	{
		//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수

		int windowNum = windows.Count - 2;
		windows.Remove(closeUiWindowObject);

		if (windowNum >= 0)
		{
			SetButtons(windows[windowNum].GetComponent<UIWindowController>().GetButtons());
		}

		Destroy(closeUiWindowObject);
	}

	public void UIWindowChildAllClose(Transform parentTransform)
	{
		//#설명#	모든 자식 UI창을 닫는 함수


		foreach (Transform child in parentTransform)
		{
			if (child.CompareTag("UIWindow"))
				child.gameObject.GetComponent<UIWindowController>().UIWindowClose();
		}
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
			Debug.Log($"할당된 버튼이 없습니다. buttons.Count : {buttons.Count}");
		}
	}

	private void SelectButton(int index)
	{
		//#설명#	주어진 인덱스의 버튼을 선택합니다.
		currentButtonIndex = index;
		if (buttons.Count > 0 && buttons[currentButtonIndex] != null)
		{
			currentButton = buttons[currentButtonIndex].gameObject;
			EventSystem.current.SetSelectedGameObject(currentButton);
		}
	}
	#endregion
}


