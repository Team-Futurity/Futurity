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

	public GameObject modalBackground;
	private Transform topCanvasTransform;

	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;

	private void Start()
	{
		if (!modalBackground)
		{
			modalBackground = new GameObject("modalBackground");
			modalBackground.AddComponent<CanvasRenderer>();
			Image image = modalBackground.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.5f);
		}

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
			Debug.Log("Top Canvas: " + topCanvas.name);
			return topCanvas.gameObject;
		}
		else
		{
			Debug.Log("No Canvas found.");
			return null;
		}
	}

	public GameObject CreateModalBackground(GameObject uiWindow)
	{
		//#����#	���� ���� Canvas���ٰ� ModalBackground�� �����ؼ� �ٸ� UI ������ ����


		modalBackground.transform.parent = topCanvasTransform;

		RectTransform modalRectTransform = modalBackground.GetComponent<RectTransform>();
		modalRectTransform.anchorMin = Vector2.zero;
		modalRectTransform.anchorMax = Vector2.one;
		modalRectTransform.offsetMin = Vector2.zero;
		modalRectTransform.offsetMax = Vector2.zero;
		modalRectTransform.SetAsLastSibling();

		modalRectTransform.SetSiblingIndex(modalRectTransform.GetSiblingIndex() - 1);

		modalBackground.SetActive(true);

		return modalBackground;
	}

	public void SetWindow(GameObject window)
	{
		windows.Add(window);
	}



	#region UIWindowOpen&Close
	public GameObject UIWindowOpen(GameObject openUiWindowObject, Transform uiParent, Vector2 instancePosition, Vector2 windowScale)
	{
		//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�


		GameObject newUI = Instantiate(openUiWindowObject, uiParent);
		if (!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;
		rectTransform.localScale = windowScale;
		windows.Add(newUI);

		SelectButton(0);
		return newUI;
	}

	public GameObject UIWindowTopOpen(GameObject openUiWindowObject, Vector2 windowPosition, Vector2 windowScale)
	{
		//#����#	UI â�� �ν��Ͻ�ȭ�ϵ�, �θ� ���� ���� Canvas�� �����Ѵ�


		GameObject newUI = Instantiate(openUiWindowObject, topCanvasTransform);
		if (!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = windowPosition;
		rectTransform.localScale = windowScale;
		windows.Add(newUI);

		SelectButton(0);

		return newUI;
	}
	public void UIWindowClose(GameObject closeUiWindowObject)
	{
		//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�

		int windowNum = windows.Count - 2;
		windows.Remove(closeUiWindowObject);
		modalBackground.SetActive(false);

		if (windowNum >= 0)
		{
			SetButtons(windows[windowNum].GetComponent<UIWindowController>().GetButtons());
			SelectButton(0);
		}

		Destroy(closeUiWindowObject);
	}

	public void UIWindowChildAllClose(Transform parentTransform)
	{
		//#����#	��� �ڽ� UIâ�� �ݴ� �Լ�


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
		this.buttons = buttons;
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
		Button currentButton = buttons[currentButtonIndex];
		ExecuteEvents.Execute(currentButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
	}

	private void SelectButton(int index)
	{
		//#����#	�־��� �ε����� ��ư�� �����մϴ�.
		currentButtonIndex = index;
		currentButton = buttons[currentButtonIndex].gameObject;
		EventSystem.current.SetSelectedGameObject(currentButton);
	}
	#endregion
}


