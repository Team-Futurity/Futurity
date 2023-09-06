using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
	private Dictionary<string, UIWindow> activeWindowDic;

	private List<string> activeWindowNameList;

	private bool isAdmin = false;

	protected override void Awake()
	{
		base.Awake();

		activeWindowDic = new Dictionary<string, UIWindow>();
		activeWindowNameList = new List<string>();
	}

	public void AddWindow(string name, UIWindow window)
	{
		activeWindowDic?.Add(name, window);
	}

	public bool GetWindowState(string name)
	{
		if(!HasWindow(name))
		{
			return false;
		}

		return true;
	}

	public bool HasWindow(string name)
	{
		bool isHas = activeWindowDic.ContainsKey(name);

		if(!isHas)
		{
			FDebug.Log($"[WindowManager] {name}에 해당하는 Window가 존재하지 않습니다. ");
		}

		return isHas;
	}

	public void CloseWindow(string name)
	{
		if(!HasWindow(name))
		{
			return;
		}

		activeWindowDic[name].gameObject.SetActive(false);
	}

	public void ShowWindow(string name)
	{
		if(!HasWindow(name))
		{
			return;
		}

		activeWindowDic[name].gameObject.SetActive(true);
	}

	public void RemoveWindow(string name)
	{
		if (!HasWindow(name))
		{
			return;
		}

		activeWindowDic.Remove(name);
	}

	public void SetAdmin(bool isOn)
	{
		isAdmin = isOn;
	}
}
