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

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.U))
		{
			AddInputActionAsset(InputActionType.Player);
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			RemoveInputActionAsset(InputActionType.Player);
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			AddInputActionAsset(InputActionType.UI);
		}

		if (Input.GetKeyDown(KeyCode.Y))
		{
			RemoveInputActionAsset(InputActionType.UI);
		}
	}

	public void AddInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionFile.Enable();

		activatedAssets.Add(data.actionFile);
	}

	public void AddInputActionAssets(params InputActionType[] types)
	{
		foreach (var type in types)
		{
			AddInputActionAsset(type);
		}
	}

	public void RemoveInputActionAsset(InputActionType type)
	{
		InputActionData data = GetActionData(type);

		data.actionFile.Disable();

		activatedAssets.Remove(data.actionFile);
	}

	public void RemoveAllInputActionAsset()
	{
		foreach (var data in activatedAssets)
		{
			data.Disable();
		}

		activatedAssets.Clear();
	}
}
