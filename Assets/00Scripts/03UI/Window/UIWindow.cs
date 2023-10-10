using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
	[field: SerializeField]
	public WindowState CurrentState { get; private set; }

	[field: SerializeField]
	public WindowList WindowType { get; private set; }

	[field: SerializeField]
	public bool PlayOnStart { get; private set; }

	private UIManager uiManager;

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

		gameObject.SetActive(true);
	}

	public void CloseWindow()
	{
		ChangeState(WindowState.ASSIGN);

		gameObject.SetActive(false);
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