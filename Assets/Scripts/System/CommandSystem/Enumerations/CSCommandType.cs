using UnityEngine.InputSystem;

public enum CSCommandType
{
    NormalAttack,
	ChargedAttack,
	Dash
}

public static class CommandEnumConverter
{
	public static PlayerInput CommandTypeToPlayerInput(CSCommandType commandType)
	{
		var playerInput = PlayerInput.None;
		switch (commandType)
		{
			case CSCommandType.NormalAttack:
				playerInput = PlayerInput.NormalAttack;
				break;
			case CSCommandType.ChargedAttack:
				playerInput = PlayerInput.SpecialAttack;
				break;
			case CSCommandType.Dash:
				playerInput = PlayerInput.Dash;
				break;
			default:
				FDebug.LogError("[CommandEnumConverter] This Type is Incorrectly Type. Please Check Your Scripts");
				break;
		}

		return playerInput;
	}
}