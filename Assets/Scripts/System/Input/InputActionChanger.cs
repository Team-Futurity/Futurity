using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionChanger : Singleton<InputActionChanger>
{
	[SerializeField] private List<InputActionData> actionDatas = new List<InputActionData>();
	private List<InputActionAsset> activatedAssets = new List<InputActionAsset>();

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
				data.actionFile.Enable();
				activatedAssets.Add(data.actionFile);
			}
			else
			{
				data.actionFile.Disable();
			}
		}
	}

	public void ActiveInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionFile.Enable();

		activatedAssets.Add(data.actionFile);
	}

	public void ActiveInputActionAssets(params InputActionType[] types)
	{
		foreach (var type in types)
		{
			ActiveInputActionAsset(type);
		}
	}

	public void DeactiveInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionFile.Disable();

		activatedAssets.Remove(data.actionFile);
	}

	public void DeactiveAllInputActionAsset()
	{
		foreach (var data in activatedAssets)
		{
			data.Disable();
		}

		activatedAssets.Clear();
	}
}
