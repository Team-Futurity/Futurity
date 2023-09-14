using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManager : Singleton<UIInputManager>
{
	private Dictionary<int, UIButton> buttonDic = new Dictionary<int, UIButton>();

	private int currentIndex = 0;

	public void SetUp()
	{

	}

	public void AddButton(int order, UIButton button)
	{
		buttonDic?.Add(order, button);
	}

	public void ClearAll()
	{
		buttonDic.Clear();
	}


	public void OnMoveToNextUI()
	{
	}

	public void OnMoveToPreviousUI()
	{
	}

	public void OnSelectUI()
	{
	}
}
