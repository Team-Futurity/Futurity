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
	//#����#	 �������� ����, ���� �� �ı��� ����ϸ�, UI ������� ��ư�� �����ϰ� �����ϴ� Ŭ�����Դϴ�.
	[Header ("������ �ý��� �Ѱ� �޴���")]
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
	/// �� �׼��� �̺�Ʈ �ڵ鷯�� �����մϴ�.
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
	/// Scene�� �ε�� ������ �ֻ��� ĵ������ �ٽ� ã�Ƽ� �����մϴ�.
	///</summary>
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		topCanvasTransform = FindTopCanvas().transform;
	}

	///<summary>
	/// ��� ĵ���� �߿��� ���� ������ ĵ������ ã�Ƽ� ��ȯ�մϴ�.
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
	/// ���ο� UI â�� �����ϰ� �θ�� ��ġ�� �����մϴ�.
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
	/// ���ο� UI â�� �����ϵ� �θ� ���� ���� Canvas�� �����մϴ�.
	///</summary>
	public GameObject WindowTopOpen(GameObject openWindowObject, bool isDeActive, Vector2 windowPosition, Vector2 windowScale)
	{
		FDebug.Log($"topCanvasTransform {topCanvasTransform}");

		GameObject newWindow = WindowOpen(openWindowObject, topCanvasTransform, isDeActive, windowPosition, windowScale);

		return newWindow;
	}

	///<summary>
	/// �÷��̾ ������� �ʴ� ��, �������⸸ �ϴ� â�� �����ϰ� ��ġ�� �����մϴ�.
	///</summary>
	public GameObject DontUsedWindowOpen(GameObject dontUsedWindow)
	{
		GameObject newWindow = Instantiate(dontUsedWindow, topCanvasTransform);

		return newWindow;
	}

	///<summary>
	/// Ư�� UI â�� �ݽ��ϴ�.
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
	/// Ư�� �θ� ������ ��� UI â�� �ݽ��ϴ�.
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
	/// windows�� �Ҵ�� ��� window�� ���������ϴ�.
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
	/// ������ ����Ʈ�� ���� �߰��մϴ�.
	///</summary>
	public void SetWindow(GameObject window)
	{
		windows.Add(window);
	}

	///<summary>
	/// ������ ����Ʈ�� �ʱ�ȭ�մϴ�.
	///</summary>
	public void ClearWindow()
	{
		windows.Clear();
	}

	#endregion

	#region SelectButton


	///<summary>
	/// �� ��ư���� �Ҵ��ϸ�, ��ư ��ȣ�� 0���� �����ϴ�.
	///</summary>
	public void SetButtons(List<Button> buttons)
	{
		this.buttons = buttons;

		SelectButton(0);
	}

	///<summary>
	/// �� �׼ǿ� ���� �̺�Ʈ �ڵ鷯�� ����մϴ�.
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
	/// UI�� Button ���� �̺�Ʈ�� �Ҵ��մϴ�.
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
	/// UI�� Button ���� �̺�Ʈ�� �����մϴ�.
	///</summary>
	public void DisableActionReference()
	{
		leftAction.action.performed -= _ => SelectPreviousButton();
		rightAction.action.performed -= _ => SelectNextButton();

		leftAction.action.Disable();
		rightAction.action.Disable();
	}

	///<summary>
	/// ���� ���õ� ��ư�� ù ��° ��ư�� �ƴ� ��� ���� ��ư�� �����մϴ�.
	///</summary>
	private void SelectPreviousButton()
	{
		if (currentButtonIndex > 0)
		{
			SelectButton(currentButtonIndex - 1);
		}
	}

	///<summary>
	/// ���� ���õ� ��ư�� ������ ��ư�� �ƴ� ��� ���� ��ư�� �����մϴ�.
	///</summary>
	private void SelectNextButton()
	{
		if (currentButtonIndex < buttons.Count - 1)
		{
			SelectButton(currentButtonIndex + 1);
		}
	}

	///<summary>
	/// ���� ���õ� ��ư�� Ŭ���մϴ�.
	///</summary>
	private void ClickCurrentButton()
	{
		if (buttons.Count != 0)
		{
			Button currentButton = buttons[currentButtonIndex];
			ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		} else
		{
			FDebug.Log($"�Ҵ�� ��ư�� �����ϴ�. buttons.Count : {buttons.Count}");
		}
	}

	///<summary>
	/// �־��� �ε����� ��ư�� �����մϴ�.
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


