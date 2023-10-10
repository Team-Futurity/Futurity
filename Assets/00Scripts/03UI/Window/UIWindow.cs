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
			FDebug.Log($"[Window] {gameObject.name}이(가) 가지고 있는 윈도우의 타입이 없습니다.");
			FDebug.Break();
		}

		// 미할당 상태로 설정
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
			FDebug.Log($"[Window] 사용하지 않는 State 입니다.");
			FDebug.Break();

			return;
		}

		CurrentState = state;
	}
}