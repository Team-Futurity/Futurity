using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
	// Active Window�� �����ϴ� Buffer

	// Window�� Data�� Dictionary�� �����ϸ鼭, ���� ���� �� Open�� ó���Ѵ�. [Admin]
	private Dictionary<string, UIWindow> activeWindowDic;

	private bool isAdmin = false;

	protected override void Awake()
	{
		base.Awake();

		activeWindowDic = new Dictionary<string, UIWindow>();
	}

	// Dictionary
	// Add, Remove, Find, Active

	public void RemoveWindow(string name)
	{
		
		activeWindowDic?.Remove(name);
	}

	public void AddWindow(string name, UIWindow window)
	{
		activeWindowDic?.Add(name, window);
	}

	public void SetAdmin(bool isOn)
	{
		isAdmin = isOn;
	}

	public bool GetWindowState(string name)
	{
		if(!HasWindow(name))
		{
			return false;
		}

		return true;
	}

	#region Admin Method
	
	public void CloseWindow(string name)
	{
		if(!isAdmin)
		{
			FDebug.Log($"[Window Manager] �ش� ������ ���� ������ �ʿ�� �մϴ�.");
			return;
		}	
		
		// Window�� ������ �ݰ��Ѵ�.
		// Window���� ���� ��ȭ�� Window �� ��ü���� �����ϵ��� �Ѵ�.
	}

	#endregion

	private bool HasWindow(string name)
	{
		bool isHas = activeWindowDic.ContainsKey(name);

		if(!isHas)
		{
			FDebug.Log($"[WindowManager] {name}�� �ش��ϴ� Window�� �������� �ʽ��ϴ�. ");
		}

		return isHas;
	}
}
