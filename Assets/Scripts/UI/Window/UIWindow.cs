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
			FDebug.Log($"[Window] {gameObject.name}이(가) 가지고 있는 윈도우의 이름이 없습니다.");
			FDebug.Break();
		}

		// WindowManager에 Window 등록하기
		WindowManager.Instance.AddWindow(WindowName, this);

		bool isSetInBuffer = WindowManager.Instance.HasWindow(WindowName);

		if (!isSetInBuffer)
		{
			FDebug.Log($"[Window] {gameObject.name}이(가) 정상적으로 등록되지 않았습니다.");
			FDebug.Break();
		}

		ChangeState(WindowState.UNASSIGN);
	}

	// Window Manager에는 등록이 되어 있으나, 보이지 않는 상태.
	public void ShowWindow()
	{
		if(CurrentState == WindowState.ASSIGN)
		{
			ChangeState(WindowState.ACTIVE);
		}
	}

	// Window의 상태를 Assign으로 변경한다.
	public void CloseWindow()
	{
		if(CurrentState == WindowState.ACTIVE)
		{
			ChangeState(WindowState.ASSIGN);
		}
	}

	public void RemoveWindow()
	{
		// Window Memory에서 지워버린 상태.
		if(CurrentState == WindowState.ACTIVE || CurrentState == WindowState.ASSIGN)
		{
			ChangeState(WindowState.UNASSIGN);
		}
	}

	private void ChangeState(WindowState state)
	{
		if(state == WindowState.NONE || state == WindowState.MAX)
		{
			FDebug.Log($"[Window] 사용하지 않는 State 입니다.");
			FDebug.Break();

			return;
		}

		if(CurrentState == state)
		{
			FDebug.Log($"[Window] 동일한 Window로 변경하고 있습니다.");
			FDebug.Break();

			return;
		}

		CurrentState = state;
	}
}
