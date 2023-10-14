using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor.PackageManager;

public class InputActionManager : Singleton<InputActionManager>
{
	/*public UnityEvent<InputActionType> OnInputActionEnabled;
	public UnityEvent<InputActionData> OnEnableEvent;
	public UnityEvent OnDisableEvent;*/
	public UnityEvent<InputActionMap> OnActionMapChange; 

	//public bool ProcessesDefaulting { get; private set; }

	public CombinedInputActions InputActions { get; private set; }

	private InputActionMap currentActionMap;

	/*[SerializeField] private List<InputActionData> actionDatas = new List<InputActionData>();
	private List<InputActionData> activatedAssets = new List<InputActionData>();

	private InputActionData GetActionData(InputActionType type) => actionDatas.First(data => data.actionType == type);*/


	protected override void Awake()
	{
		base.Awake();

		InputActions = new CombinedInputActions();
		//ProcessesDefaulting = false;

		//SetDefault();
	}

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
		action.started += callback;

		if(!isButton)
		{
			action.performed += callback;
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
