using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : Singleton<UIInputManager>
{
	// Button List
	private Dictionary<int, UIButton> currentActiveButtons = new Dictionary<int, UIButton>();

	// Button Index
	private int currentIndex = 0;
	private int saveIndex = 0;
	private int maxMoveIndex = 0;

	private bool isUnableMoveButton = false;

	private void Start()
	{
		maxMoveIndex = -1;
		
		CombinedInputActions.UIBehaviourActions map = InputActionManager.Instance.InputActions.UIBehaviour;
		InputActionManager.Instance.ToggleActionMap(map);
		InputActionManager.Instance.RegisterCallback(map.MoveToPreviousUI, (context) => OnMoveToPreviousUI(context), true);
		InputActionManager.Instance.RegisterCallback(map.MoveToNextUI, (context) => OnMoveToNextUI(context), true);
		InputActionManager.Instance.RegisterCallback(map.ClickUI, (context) => OnClickUI(context), true);
		
		// Left & Right
		InputActionManager.Instance.RegisterCallback(map.LeftKey, (context) => OnLeftKey(context), true);
		InputActionManager.Instance.RegisterCallback(map.RightKey, (context) => OnRightKey(context), true);
		
		// Esc Key
		InputActionManager.Instance.RegisterCallback(map.ESC, (context) => OnESC(context), true);
		
		InputActionManager.Instance.RegisterCallback(map.ExitKey, (context) => OnExitKey(context), true);
	}

	#region Button

	public void SetButtonList(List<UIButton> buttons, bool isDefaultFocus = true)
	{
		for (int i = 0; i < buttons.Count; ++i)
		{
			currentActiveButtons?.Add(i, buttons[i]);
			buttons[i].Init();
		}

		if (isDefaultFocus)
		{
			DefaultFocus();
		}
	}

	public void SetDefaultFocusForced(int index = -1)
	{
		currentActiveButtons[currentIndex].Select(false);

		if (index != -1)
			currentIndex = index;
		
		SelectUI();
	}

	public void DefaultFocus()
	{
		currentIndex = 0;
		SelectUI();
	}


	public void InitAll()
	{
		foreach (var button in currentActiveButtons)
		{
			button.Value.SetDefault();
		}
	}
	
	public void ClearAll()
	{
		currentActiveButtons.Clear();
	}

	public void SelectUI()
	{
		if(!currentActiveButtons.ContainsKey(currentIndex))
		{
			FDebug.Log($"버튼이 없다.", GetType());
			return;
		}
		currentActiveButtons[currentIndex].Select(true);
	}

	public void SetUnableMoveButton(bool isOn)
	{
		isUnableMoveButton = isOn;
	}

	public void SaveIndex()
	{
		if (saveIndex > 0)
			return;
		
		saveIndex = currentIndex;
	}

	public void SetMaxMoveIndex(int index)
	{
		maxMoveIndex = index;
	}

	public void SetSaveIndexToCurrentIndex()
	{
		if (saveIndex < 0)
		{
			FDebug.Log("Save처리 된 Index가 존재하지 않음.");
		}
		
		currentIndex = saveIndex;
		saveIndex = -1;
		SelectUI();
	}

	private void ChangeToIndex(int num)
	{
		var result = currentIndex + num;

		if (result < 0 || result >= currentActiveButtons.Count || (maxMoveIndex > result && maxMoveIndex != -1))
		{
			return;
		}

		currentActiveButtons[currentIndex].Select(false);
		currentIndex = result;
	}

	#endregion

	#region Input Action

	public void OnMoveToNextUI(InputAction.CallbackContext context)
	{
		if (isUnableMoveButton) { return;}
		ChangeToIndex(1);

		SelectUI();
	}

	public void OnMoveToPreviousUI(InputAction.CallbackContext context)
	{
		if (isUnableMoveButton) { return;}
		
		ChangeToIndex(-1);

		SelectUI();
	}

	public void OnLeftKey(InputAction.CallbackContext context)
	{
		if (!currentActiveButtons[currentIndex].usedLeftRight || !currentActiveButtons.ContainsKey(currentIndex))
			return;

		currentActiveButtons[currentIndex].OnLeft();
	}

	public void OnRightKey(InputAction.CallbackContext context)
	{
		if (!currentActiveButtons[currentIndex].usedLeftRight || !currentActiveButtons.ContainsKey(currentIndex))
			return;

		currentActiveButtons[currentIndex].OnRight();
	}

	public void OnClickUI(InputAction.CallbackContext context)
	{
		if (currentActiveButtons == null || !currentActiveButtons.ContainsKey(currentIndex))
		{
			return;
		}

		currentActiveButtons[currentIndex].Active();
	}
	
	public void OnExitKey(InputAction.CallbackContext context)
	{
		if (UIManager.Instance.IsOpenWindow(WindowList.PART_EQUIP) || UIManager.Instance.IsOpenWindow(WindowList.PART_EQUIP_SELECT))
		{
			SaveIndex();
			UIManager.Instance.OpenWindow(WindowList.PART_EXIT);
		}
	}

	public void OnESC(InputAction.CallbackContext context)
	{
		if (!UIManager.Instance.HasWindow(WindowList.PAUSE))
		{
			return;
		}
		
		if (UIManager.Instance.IsOpenWindow(WindowList.PAUSE))
		{
			return;
		}

		SaveIndex();
		Time.timeScale = .0f;
		UIManager.Instance.OpenWindow(WindowList.PAUSE);
	}
	#endregion
}
