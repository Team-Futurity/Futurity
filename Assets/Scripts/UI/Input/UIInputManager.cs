using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : Singleton<UIInputManager>
{
	private Dictionary<int, UIButton> buttonDic = new Dictionary<int, UIButton>();
	private PlayerInput playerInput;

	private int currentIndex = 0;

	protected override void Awake()
	{
		base.Awake();

		TryGetComponent(out playerInput);

		playerInput.ActivateInput();
	}

	private void Start()
	{
		InputActionManager.Instance.OnEnableEvent.AddListener(SetInputActionAsset);
		InputActionManager.Instance.OnDisableEvent.AddListener(RemoveInputActionAsset);
	}

	private void SetInputActionAsset(InputActionData 액션데이터)
	{
		if (액션데이터.actionType == InputActionType.UI)
		{
			playerInput.actions = 액션데이터.actionAsset;
		}
	}

	private void RemoveInputActionAsset()
	{
		playerInput.actions = null;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			InputActionManager.Instance.DisableAllInputActionAsset();
			InputActionManager.Instance.EnableInputActionAsset(InputActionType.Player);
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			InputActionManager.Instance.DisableAllInputActionAsset();
			InputActionManager.Instance.EnableInputActionAsset(InputActionType.UI);
		}
	}

	public void SetInputAction(InputActionAsset asset)
	{
		playerInput.actions = asset;
	}

	public void AddButton(int order, UIButton button)
	{
		buttonDic?.Add(order, button);
	}

	public void ClearAll()
	{
		buttonDic.Clear();
	}

	public void SelectUI()
	{
		buttonDic[currentIndex].Select();
	}

	private void ChangeToIndex(int num)
	{
		var result = currentIndex + num;

		if (result < 0 || result >= buttonDic.Count)
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
		if (buttonDic == null)
		{
			return;
		}

		buttonDic[currentIndex].Active();
	}
	#endregion
}
