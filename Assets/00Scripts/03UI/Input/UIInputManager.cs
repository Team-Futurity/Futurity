using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : Singleton<UIInputManager>
{
	// Button List
	private Dictionary<int, UIButton> currentActiveButtons = new Dictionary<int, UIButton>();

	private PlayerInput playerInput;

	// Button Index
	private int currentIndex = 0;
	private int saveIndex = 0;

	protected override void Awake()
	{
		base.Awake();

		TryGetComponent(out playerInput);
	}

	private void Start()
	{
		CombinedInputActions.UIBehaviourActions map = InputActionManager.Instance.InputActions.UIBehaviour;
		InputActionManager.Instance.ToggleActionMap(map);
		InputActionManager.Instance.RegisterCallback(map.MoveToPreviousUI, (context) => OnMoveToPreviousUI(context), true);
		InputActionManager.Instance.RegisterCallback(map.MoveToNextUI, (context) => OnMoveToNextUI(context), true);
		InputActionManager.Instance.RegisterCallback(map.ClickUI, (context) => OnClickUI(context), true);
		
		// Left & Right
		InputActionManager.Instance.RegisterCallback(map.LeftKey, (context) => OnLeftKey(context), true);
		InputActionManager.Instance.RegisterCallback(map.RightKey, (context) => OnRightKey(context), true);
	}

	private void SetInputActionAsset(InputActionData actionData)
	{
		if (actionData.actionType == InputActionType.UI)
		{
			playerInput.actions = actionData.actionAsset;
			playerInput.actions.Enable();
		}
	}

	private void RemoveInputActionAsset()
	{
		playerInput.actions.Disable();
	}

	#region Button

	public void SetButtonList(List<UIButton> buttons, bool isDefaultFocus = true)
	{
		for (int i = 0; i < buttons.Count; ++i)
		{
			currentActiveButtons?.Add(i, buttons[i]);
			Debug.Log(buttons[i].transform.name);
		}

		if (isDefaultFocus)
		{
			DefaultFocus();
		}
	}

	public void DefaultFocus()
	{
		currentIndex = 0;
		SelectUI();
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

	public void SaveIndex()
	{
		saveIndex = currentIndex;
	}

	public void SetSaveIndexToCurrentIndex()
	{
		if (saveIndex < 0)
		{
			FDebug.Log("Save처리 된 Index가 존재하지 않음.");
		}
		
		currentIndex = saveIndex;
		saveIndex = -1;
	}

	private void ChangeToIndex(int num)
	{
		var result = currentIndex + num;

		if (result < 0 || result >= currentActiveButtons.Count)
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
		ChangeToIndex(1);

		SelectUI();
	}

	public void OnMoveToPreviousUI(InputAction.CallbackContext context)
	{
		ChangeToIndex(-1);

		SelectUI();
	}

	public void OnLeftKey(InputAction.CallbackContext context)
	{
		if (!currentActiveButtons[currentIndex].usedLeftRight)
			return;

		currentActiveButtons[currentIndex].OnLeft();
	}

	public void OnRightKey(InputAction.CallbackContext context)
	{
		if (!currentActiveButtons[currentIndex].usedLeftRight)
			return;

		currentActiveButtons[currentIndex].OnRight();
	}

	public void OnClickUI(InputAction.CallbackContext context)
	{
		if (currentActiveButtons == null)
		{
			return;
		}

		currentActiveButtons[currentIndex].Active();
	}
	#endregion
}
