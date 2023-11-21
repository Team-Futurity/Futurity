using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	private Dictionary<WindowList, UIWindow> windowDic;
	private List<WindowList> activeWindowList;

	private Stack<UIWindow> windowStack;

	public List<UIWindow> defaultWindow = new List<UIWindow>();

	protected override void Awake()
	{
		base.Awake();

		windowDic = new Dictionary<WindowList, UIWindow>();
		activeWindowList = new List<WindowList>();
		windowStack = new Stack<UIWindow>();
	}
	
	#region Window

	public void OpenDefaultWindow()
	{
		for(int i=0;i<defaultWindow.Count;++i)
		{
			defaultWindow[i].OpenWindow();
			AddWindow(defaultWindow[i].WindowType, defaultWindow[i]);
		}
	}

	public void CloseDefaultWindow()
	{
		for (int i = 0; i < defaultWindow.Count; ++i)
		{
			defaultWindow[i].CloseWindow();
			CloseWindow(defaultWindow[i].WindowType);
		}
	}

	public void RemoveWindow()
	{
		windowDic.Clear();
		activeWindowList.Clear();
	}

	public void AddWindow(WindowList type, UIWindow window)
	{
		windowDic?.Add(type, window);
		windowStack?.Push(window);
		
		window.SetUp();
	}

	public void OpenWindow(WindowList type)
	{
		activeWindowList?.Add(type);
		windowDic[type].OpenWindow();
	}

	public void CloseWindow(WindowList type)
	{
		activeWindowList?.Remove(type);
		windowDic[type].CloseWindow();
	}

	public void RefreshWindow(WindowList type)
	{
		windowDic[type].RefreshWindow();
	}

	public WindowList GetBefroeWindow()
	{
		if (0 >= activeWindowList.Count)
			return WindowList.NONE;
		else
			return activeWindowList[^1];
	}
	
	public void OpenWindowAllClose()
	{
		CloseOtherWindow(WindowList.MAX);
	}

	public void CloseOtherWindow(params WindowList[] windows)
	{
		var exceptionWindow = activeWindowList.ConvertAll(x => x);

		if (windows[0] != WindowList.MAX)
		{
			exceptionWindow.Except(windows.ToList());
		}

		foreach (var closeWindow in exceptionWindow)
		{
			CloseWindow(closeWindow);
		}
	}

	public void RemoveAllWindow()
	{
		windowDic.Clear();
	}

	public bool HasWindow(WindowList type)
	{
		return windowDic.ContainsKey(type);
	}

	public bool IsOpenWindow(WindowList type)
	{
		return windowDic[type].isOpen;
	}
	
	#endregion
}
