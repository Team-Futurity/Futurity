using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIWindow : MonoBehaviour
{
	[field: SerializeField]
	public WindowState CurrentState { get; private set; }

	[field: SerializeField]
	public WindowList WindowType { get; private set; }

	[field: SerializeField]
	public bool PlayOnStart { get; private set; }

	private UIManager uiManager;

	[HideInInspector]
	public UnityEvent<bool> onOpen;

	[field: SerializeField, Space(10)]
	public List<UIButton> CurrentWindowButtons { get; private set; }
	
	[field: SerializeField, Space(10)]
	public List<GameObject> CurrentWindowObjects { get; private set; }

	public bool isOpen = false;

	private void Awake()
	{
		if (WindowType == WindowList.NONE)
		{
			FDebug.Log($"[Window] {gameObject.name}��(��) ������ �ִ� �������� Ÿ���� �����ϴ�.");
			FDebug.Break();
		}

		// ���Ҵ� ���·� ����
		ChangeState(WindowState.UNASSIGN);
	}

	private void Start()
	{
		uiManager = UIManager.Instance;
		
		uiManager.AddWindow(WindowType, this);

		if (PlayOnStart)
		{
			uiManager.OpenWindow(WindowType);
		}
		else
		{
			uiManager.CloseWindow(WindowType);
		}
	}

	public void SetUp()
	{
		ChangeState(WindowState.ASSIGN);
	}

	public void OpenWindow()
	{
		ChangeState(WindowState.ACTIVE);

		if (isOpen) return;

		isOpen = true;

		// Button Clear
		UIInputManager.Instance.ClearAll();

		// Set this CurrentButton -> UIInputManager
		if (CurrentWindowButtons != null)
		{
			UIInputManager.Instance.SetButtonList(CurrentWindowButtons);
		}

		onOpen?.Invoke(true);

		for (int i = 0; i < CurrentWindowObjects.Count; ++i)
		{
			CurrentWindowObjects[i].SetActive(true);
		}
	}

	public void RefreshWindow()
	{
		ChangeState(WindowState.ACTIVE);

		// Button Clear
		UIInputManager.Instance.ClearAll();
		
		// Set this CurrentButton -> UIInputManager
		UIInputManager.Instance.SetButtonList(CurrentWindowButtons, false);
	}

	public void CloseWindow()
	{
		ChangeState(WindowState.ASSIGN);

		for (int i = 0; i < CurrentWindowObjects.Count; ++i)
		{
			CurrentWindowObjects[i].SetActive(false);
		}

		isOpen = false;

		for (int i = 0; i < CurrentWindowButtons.Count; ++i)
		{
			CurrentWindowButtons[i].SetDefault();
		}
		
		//onOpen?.Invoke(false);
	}

	public void RemoveWindow()
	{
		if (CurrentState == WindowState.ACTIVE || CurrentState == WindowState.ASSIGN)
		{
			CloseWindow();

			ChangeState(WindowState.UNASSIGN);
		}
	}

	private void ChangeState(WindowState state)
	{
		if (state == WindowState.NONE || state == WindowState.MAX)
		{
			FDebug.Log($"[Window] ������� �ʴ� State �Դϴ�.");
			FDebug.Break();

			return;
		}

		CurrentState = state;
	}
}