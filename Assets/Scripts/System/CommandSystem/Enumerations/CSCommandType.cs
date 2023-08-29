using UnityEngine.InputSystem;

public enum CSCommandType
{
    NormalAttack,
	ChargedAttack,
	Dash
}

public static class CommandEnumConverter
{
	public static PlayerInputEnum CommandTypeToPlayerInput(CSCommandType commandType)
	{
		var playerInput = PlayerInputEnum.None;
		switch (commandType)
		{
			case CSCommandType.NormalAttack:
				playerInput = PlayerInputEnum.NormalAttack;
				break;
			case CSCommandType.ChargedAttack:
				playerInput = PlayerInputEnum.SpecialAttack;
				break;
			case CSCommandType.Dash:
				playerInput = PlayerInputEnum.Dash;
				break;
			default:
				FDebug.LogError("[CommandEnumConverter] This Type is Incorrectly Type. Please Check Your Scripts");
				break;
		}

		return playerInput;
	}
}