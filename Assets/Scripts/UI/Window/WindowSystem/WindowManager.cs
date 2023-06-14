using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Events;

public class WindowManager : Singleton<WindowManager>
{
	//#����#	������ �Ѱ� �޴����� Window�� ���� �ݴ°��� ����
	[Header ("������ �ý��� �Ѱ� �޴���")]
	[Space(15)]

	[Tooltip("���� �����ִ� window��")]
	public List<GameObject> windows = new List<GameObject>();
	[Tooltip("���� �������� window�� Buttons")]
	public List<Button> buttons;


	private int currentButtonIndex;					// ���� �����ִ� ��ư�� ��ȣ
	[SerializeField]
	private GameObject currentButton;				// ���� �����ִ� ��ư
	
	private float holdTime;                         // ��ư�� ������ �ִ� �ð�
	[SerializeField]
	private float holdThreshold = 1f;               // ��ư�� ��� ������ �ð� �Ӱ谪 ����.

	[Tooltip("�� ��ܿ� ��ġ�ϰ� �ִ� �˹����� Transform")]
	public Transform topCanvasTransform;

	[Space(15)]
	[Header("Window�� �����ϴ� Action")]
	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;

	private Coroutine holdCorutine;					// Ȧ�� �ð��� ����ϱ� ���� �ڷ�ƾ

	protected override void Awake()
	{
		base.Awake();

		topCanvasTransform = FindTopCanvas().transform;

		SceneManager.sceneLoaded += OnSceneLoaded;
		SelectButton(0);
	}

	/// <summary>
	/// �� �׼��� �̺�Ʈ �ڵ鷯�� �����մϴ�.
	/// </summary>
	private void OnDisable()
	{
		base.OnDisable();

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
			FDebug.Log($"allCanvases : {canvas}");

			if (canvas.transform.root == canvas.transform) continue;

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

	/// <summary>
	/// �־��� �̸��� window�� ã���ϴ�.
	/// </summary>
	/// <param name="windowName">ã���� �ϴ� window�� �̸�</param>
	/// <returns>ã�� window. ���� ã�� ���ϸ� null�� ��ȯ�մϴ�.</returns>
	public GameObject FindWindow(string windowName)
	{
		foreach (GameObject window in windows)
		{
			if (window.name == windowName)
			{
				return window;
			}
		}

		return null;
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
	/// ���ο� UI â�� �����ϰ� �θ�� ��ġ�� �����մϴ�.
	///</summary>
	#region UIWindowOpen&Close
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
		
		selectAction.action.started += _ => StartHold();
		selectAction.action.canceled += _ => EndHold();
		

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
		} else
		{
			SelectButton(buttons.Count - 1);
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
		} else
		{
			SelectButton(0);
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

	#region HoldButton 
	/// <summary>
	/// ��ư�� ���� ������ �ð��� ����մϴ�.
	/// </summary>
	private void StartHold()
	{
		holdTime = 0;
		if (holdCorutine != null)
		{
			StopCoroutine(holdCorutine);
		}

		if (buttons != null && buttons.Count > 0)
		{
			if (buttons[currentButtonIndex] != null)
			{
				holdCorutine = StartCoroutine(CheckHoldDuration());
				Debug.Log($"{buttons[currentButtonIndex].gameObject.name}�� hold ����`");
			}
		}
	}

	/// <summary>
	/// ��ư�� �������� holdCorutine�� �����մϴ�. ���ص� ��ư�� OnDehold�� �ǽ��մϴ�.
	/// </summary>
	private void EndHold()
	{
		if (holdCorutine != null)
		{
			StopCoroutine(holdCorutine);
		}

			if (buttons != null && buttons.Count > 0)
		{
			if (buttons[currentButtonIndex] != null)
			{
				currentButton = buttons[currentButtonIndex].gameObject;

				currentButton.GetComponent<UIWindowButtonController>()?.OnDehold();
				Debug.Log($"{currentButton.name}�� hold �̺�Ʈ�� ����Ǿ����ϴ�.");
			}
		}
	}

	/// <summary>
	/// Ȧ�� �ð��� ����մϴ�. Ȧ�� �ð��� �Ӱ谪�� �Ѿ�ٸ�, Ȧ�� �̺�Ʈ�� ȣ���մϴ�.
	/// </summary>
	IEnumerator CheckHoldDuration()
	{
		while (true) {
			yield return true;
			holdTime += Time.deltaTime;
			Debug.Log($"{currentButton.name}�� {holdTime}���� hold �Ǿ����ϴ�.");

			if (holdTime >= holdThreshold) // �Ӱ谪�� �Ѿ�ٸ�,
			{
				if (buttons != null && buttons.Count > 0)
				{
					if (buttons[currentButtonIndex] != null)
					{
						currentButton = buttons[currentButtonIndex].gameObject;

						currentButton.GetComponent<UIWindowButtonController>()?.OnHold();
						Debug.Log($"{currentButton.name}�� hold �̺�Ʈ�� ��µǾ����ϴ�.");

						break;
					}
				}
			}
		}
	}
	#endregion

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


