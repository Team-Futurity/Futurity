using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
	[field: SerializeField]
	public WindowState CurrentState { get; private set; }
	
	[field: SerializeField]
	public string WindowName { get; private set; }

	public void Awake()
	{
		if(WindowName == "")
		{
			FDebug.Log($"[Window] {gameObject.name}��(��) ������ �ִ� �������� �̸��� �����ϴ�.");
			FDebug.Break();
		}

		// WindowManager�� Window ����ϱ�
		WindowManager.Instance.AddWindow(WindowName, this);

		bool isSetInBuffer = WindowManager.Instance.HasWindow(WindowName);

		if (!isSetInBuffer)
		{
			FDebug.Log($"[Window] {gameObject.name}��(��) ���������� ��ϵ��� �ʾҽ��ϴ�.");
			FDebug.Break();
		}

		ChangeState(WindowState.UNASSIGN);
	}

	// Window Manager���� ����� �Ǿ� ������, ������ �ʴ� ����.
	public void ShowWindow()
	{
		if(CurrentState == WindowState.ASSIGN)
		{
			ChangeState(WindowState.ACTIVE);
		}
	}

	// Window�� ���¸� Assign���� �����Ѵ�.
	public void CloseWindow()
	{
		if(CurrentState == WindowState.ACTIVE)
		{
			ChangeState(WindowState.ASSIGN);
		}
	}

	public void RemoveWindow()
	{
		// Window Memory���� �������� ����.
		if(CurrentState == WindowState.ACTIVE || CurrentState == WindowState.ASSIGN)
		{
			ChangeState(WindowState.UNASSIGN);
		}
	}

	private void ChangeState(WindowState state)
	{
		if(state == WindowState.NONE || state == WindowState.MAX)
		{
			FDebug.Log($"[Window] ������� �ʴ� State �Դϴ�.");
			FDebug.Break();

			return;
		}

		if(CurrentState == state)
		{
			FDebug.Log($"[Window] ������ Window�� �����ϰ� �ֽ��ϴ�.");
			FDebug.Break();

			return;
		}

		CurrentState = state;
	}
}
