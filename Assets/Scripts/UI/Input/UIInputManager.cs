using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManager : Singleton<UIInputManager>
{
	private Dictionary<int, UIButton> buttonDic = new Dictionary<int, UIButton>();

	private int currentIndex = 0;

	private void Start()
	{
// 		InputActionManager.Instance.OnInput
	}

	public void SetUp()
	{
		SelectUI();
	}

	public void AddButton(int order, UIButton button)
	{
		buttonDic?.Add(order, button);
	}

	public void ClearAll()
	{
		buttonDic.Clear();
	}

	private void SelectUI()
	{
		buttonDic[currentIndex].Select();
	}
	
	private void ChangeToIndex(int num)
	{
		var result = currentIndex + num;

		if(result < 0 || result >= buttonDic.Count)
		{
			return;
		}

		currentIndex = result;
	}

	#region Input Action

	public void OnMoveToNextUI()
	{
		ChangeToIndex(1);

		SelectUI();
	}

	public void OnMoveToPreviousUI()
	{
		ChangeToIndex(-1);

		SelectUI();
	}

	public void OnClickUI()
	{
		buttonDic[currentIndex].Active();
	}
	#endregion
}
