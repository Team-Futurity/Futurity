using System;
using UnityEngine.InputSystem;

public enum InputActionType
{
	None,
	Player,
	UI
}

[Serializable]
public struct InputActionData
{
	public InputActionType actionType;
	public InputActionAsset actionFile;
}