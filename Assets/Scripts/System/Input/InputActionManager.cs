using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionManager : Singleton<InputActionManager>
{
	[SerializeField] private List<InputActionData> actionDatas = new List<InputActionData>();
	private List<InputActionData> activatedAssets = new List<InputActionData>();

	private InputActionData GetActionData(InputActionType type) => actionDatas.First(data => data.actionType == type);

	private void Start()
	{
		SetDefault();
	}

	public void SetDefault()
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
	}

	public void EnableInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionAsset.Enable();

		activatedAssets.Add(data);
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
			data.actionAsset.Disable();
		}

		activatedAssets.Clear();
	}

	public bool IsActive(InputActionType type) => activatedAssets.Count(data => data.actionType == type) > 0;
}
