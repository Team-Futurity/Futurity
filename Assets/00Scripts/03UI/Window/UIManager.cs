using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	private Dictionary<WindowList, UIWindow> windowDic;
	private List<WindowList> activeWindowList;

	protected override void Awake()
	{
		base.Awake();

		windowDic = new Dictionary<WindowList, UIWindow>();
		activeWindowList = new List<WindowList>();
	}
	
	#region Window

	public void AddWindow(WindowList type, UIWindow window)
	{
		windowDic?.Add(type, window);
		
		window.SetUp();
	}

	public void RemoveWindow(WindowList type)
	{
		var isOk = windowDic?.Remove(type);

		if (isOk == false)
		{
			FDebug.Log($"{type}에 해당하는 Window가 존재하지 않거나, 잘못되었습니다.", GetType());
		}
		
		windowDic[type].RemoveWindow();
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
			windowDic[closeWindow].CloseWindow();
		}
	}

	public bool HasWindow(WindowList type)
	{
		return windowDic.ContainsKey(type);
	}
	
	#endregion

	#region Canvas
	
	
	
	#endregion
}
