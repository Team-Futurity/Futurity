using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
	[field: SerializeField] public WindowState CurrentState { get; private set; }

	[field: SerializeField] public string WindowName { get; private set; }

	[field: SerializeField] public bool PlayOnStart { get; private set; }

	private void Awake()
	{
		if (WindowName == "")
		{
			FDebug.Log($"[Window] {gameObject.name}��(��) ������ �ִ� �������� �̸��� �����ϴ�.");
			FDebug.Break();
		}
	}

	private void Start()
	{
		WindowManager.Instance.AddWindow(WindowName, this);

		bool isSetInBuffer = WindowManager.Instance.HasWindow(WindowName);

		if (!isSetInBuffer)
		{
			FDebug.Log($"[Window] {gameObject.name}��(��) ���������� ��ϵ��� �ʾҽ��ϴ�.");
			FDebug.Break();
		}

		if (PlayOnStart)
		{
			WindowManager.Instance.ShowWindow(WindowName);
		}
		else
		{
			WindowManager.Instance.CloseWindow(WindowName);
		}
	}

	public void SetUp()
	{
		ChangeState(WindowState.ASSIGN);
	}

	public void ShowWindow()
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