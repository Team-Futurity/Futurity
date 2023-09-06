using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
	// Active Window를 관리하는 Buffer

	// Window의 Data를 Dictionary로 관리하면서, 강제 종료 및 Open을 처리한다. [Admin]
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
			FDebug.Log($"[Window Manager] 해당 권한은 어드민 권한이 필요로 합니다.");
			return;
		}	
		
		// Window를 강제로 닫게한다.
		// Window들의 상태 변화는 Window 각 개체에서 진행하도록 한다.
	}

	#endregion

	private bool HasWindow(string name)
	{
		bool isHas = activeWindowDic.ContainsKey(name);

		if(!isHas)
		{
			FDebug.Log($"[WindowManager] {name}에 해당하는 Window가 존재하지 않습니다. ");
		}

		return isHas;
	}
}
