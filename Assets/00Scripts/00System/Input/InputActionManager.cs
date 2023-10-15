using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionManager : Singleton<InputActionManager>
{
	/*public UnityEvent<InputActionType> OnInputActionEnabled;
	public UnityEvent<InputActionData> OnEnableEvent;
	public UnityEvent OnDisableEvent;*/
	public UnityEvent<InputActionMap> OnActionMapChange; 

	//public bool ProcessesDefaulting { get; private set; }

	public CombinedInputActions InputActions { get; private set; }

	private InputActionMap currentActionMap;

	[SerializeField] private bool isTestMode;
	[SerializeField] private bool onPlayer;
	[SerializeField] private bool onUIBehaviour;
	[SerializeField] private bool onDebug;


	protected override void Awake()
	{
		base.Awake();

		InputActions = new CombinedInputActions();
		//ProcessesDefaulting = false;

		//SetDefault();
	}

#if UNITY_EDITOR
	private void Update()
	{
		if(isTestMode)
		{
			if(onPlayer)
			{
				ToggleActionMap(InputActions.Player);
				return;
			}

			if (onUIBehaviour)
			{
				ToggleActionMap(InputActions.UIBehaviour);
				return;
			}

			if (onDebug)
			{
				ToggleActionMap(InputActions.Debug);
				return;
			}
		}
	}
#endif

	public void ToggleActionMap(InputActionMap map)
	{
		if (map.enabled) { return; }

		currentActionMap = map;

		InputActions.Disable(); // 모든 ActionMap Disable
		OnActionMapChange?.Invoke(map);
		map.Enable();
	}

	public void DisableActionMap()
	{
		if (!currentActionMap.enabled) { return; }

		InputActions.Disable(); // 모든 ActionMap Disable
	}

	public void RegisterCallback(InputAction action, Action<InputAction.CallbackContext> callback, bool isButton = false)
	{
		// 빼고 넣는 건 callback이 중복으로 들어가는 걸 방지하기 위함!
		//action.started -= callback;
		action.started += callback;

		if(!isButton)
		{
			//action.performed -= callback;
			action.performed += callback;

			//action.canceled -= callback;
			action.canceled += callback;
		}
	}

	public void RemoveCallback(InputAction action, Action<InputAction.CallbackContext> callback, bool isButton = false)
	{
		action.started -= callback;

		if(!isButton)
		{
			action.performed -= callback;
			action.canceled -= callback;
		}
	}

	/*public void SetDefault()
	{
		activatedAssets.Clear();

		foreach (var data in actionDatas)
		{
			if (data.canRunDefault)
			{
				data.actionAsset.Enable();
				activatedAssets.Add(data);
			}
			else
			{
				data.actionAsset.Disable();
			}
		}

		ProcessesDefaulting = true;
	}

	public void EnableInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionAsset.Enable();

		activatedAssets.Add(data);

		OnInputActionEnabled.Invoke(type);
		OnEnableEvent?.Invoke(data);
	}

	public void EnableInputActionAssets(params InputActionType[] types)
	{
		foreach (var type in types)
		{
			EnableInputActionAsset(type);
		}
	}

	public void DisableInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionAsset.Disable();

		activatedAssets.Remove(data);
	}

	public void DisableInputActionAssets(params InputActionType[] types)
	{
		foreach (var type in types)
		{
			DisableInputActionAsset(type);
		}
	}

	public void DisableAllInputActionAsset()
	{
		foreach (var data in activatedAssets)
		{
			OnDisableEvent?.Invoke();
			data.actionAsset.Disable();
		}

		activatedAssets.Clear();
	}

	public bool IsActive(InputActionType type) => activatedAssets.Count(data => data.actionType == type) > 0;

	public UnityEngine.InputSystem.InputActionAsset GetByType(InputActionType type)
	{
		return GetActionData(type).actionAsset;
	}*/
}
