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
	//#����#	������ �Ѱ� �޴����� Window�� ���� �ݴ°��� ����
	[Header ("������ �ý��� �Ѱ� �޴���")]
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
		//#����#	�� �׼��� �̺�Ʈ �ڵ鷯�� �����մϴ�.


		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();
		selectAction.action.performed -= _ => ClickCurrentButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
		selectAction.action.Disable();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//#����#	Scene�� �ε�� ������ topCanvasTransform�� �ٽ� �����ϱ� ���� FindTopCanvas�� ȣ���Ѵ�.


		topCanvasTransform = FindTopCanvas().transform;
	}

	private GameObject FindTopCanvas()
	{
		//#����#	���� ���� Canvas�� ã�Ƽ� topCanvasTransform�� �Ҵ���


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
		//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�


		GameObject newWindow = Instantiate(hpBarObject, topCanvasTransform);

		return newWindow;
	}

	#region UIWindowOpen&Close
	public GameObject WindowOpen(GameObject openUiWindowObject, Transform WindowParent, bool isDeActive, Vector2 windowPosition, Vector2 windowScale)
	{
		//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�


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
		//#����#	UI â�� �ν��Ͻ�ȭ�ϵ�, �θ� ���� ���� Canvas�� �����Ѵ�

		FDebug.Log($"topCanvasTransform {topCanvasTransform.name}");

		GameObject newWindow = WindowOpen(openWindowObject, topCanvasTransform, isDeActive, windowPosition, windowScale);

		return newWindow;
	}
	public void WindowClose(GameObject closeUiWindowObject)
	{
		//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�

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
		//#����#	��� �ڽ� UIâ�� �ݴ� �Լ�


		foreach (Transform child in parentTransform)
		{
			if (child.CompareTag("UIWindow"))
				child.gameObject.GetComponent<WindowController>().WindowClose();
		}
	}

	public void WindowsClearner()
	{
		//#����#	windows�� �Ҵ�� ��� window�� ����� �Լ�
		
	}
	#endregion


	#region SelectButton
	public void SetButtons(List<Button> buttons)
	{
		//#����#	�� ��ư���� �Ҵ��ϸ�, ��ư ��ȣ�� 0���� ������.
		this.buttons = buttons;

		SelectButton(0);
	}

	public void SetInputActionReference(InputActionReference leftAction, InputActionReference rightAction, InputActionReference selectAction)
	{
		//#����#	�� �׼ǿ� ���� �̺�Ʈ �ڵ鷯�� ����մϴ�.
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
		//#����#	�� �׼ǿ� ���� �̺�Ʈ �ڵ鷯�� �Ҵ��մϴ�.
		DisableActionReference();

		leftAction.action.performed += _ => SelectPreviousButton();
		rightAction.action.performed += _ => SelectNextButton();

		leftAction.action.Enable();
		rightAction.action.Enable();
	}
	public void DisableActionReference()
	{
		//#����#	�� �׼ǿ� ���� �̺�Ʈ �ڵ鷯�� �����մϴ�.
		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
	}

	private void SelectPreviousButton()
	{
		//#����#	���� ���õ� ��ư�� ù ��° ��ư�� �ƴ� ��� ���� ��ư�� �����մϴ�.
		if (currentButtonIndex > 0)
		{
			SelectButton(currentButtonIndex - 1);
		}
	}

	private void SelectNextButton()
	{
		//#����#	���� ���õ� ��ư�� ������ ��ư�� �ƴ� ��� ���� ��ư�� �����մϴ�.
		if (currentButtonIndex < buttons.Count - 1)
		{
			SelectButton(currentButtonIndex + 1);
		}
	}

	private void ClickCurrentButton()
	{
		//#����#	���� ���õ� ��ư�� Ŭ���մϴ�.
		if (buttons.Count != 0)
		{
			Button currentButton = buttons[currentButtonIndex];
			ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		} else
		{
			FDebug.Log($"�Ҵ�� ��ư�� �����ϴ�. buttons.Count : {buttons.Count}");
		}
	}

	private void SelectButton(int index)
	{
		//#����#	�־��� �ε����� ��ư�� �����մϴ�.
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


