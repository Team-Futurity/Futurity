using UnityEngine;

public enum SpecialMoveType
{
	None,
	Basic = 2201,
	Beta = 2202,
	Test
}

[System.Serializable]
public class ActivePartData
{
	public SpecialMoveType type;
	[SerializeReference]
	public SpecialMoveProcessor proccessor;

	public ActivePartData(SpecialMoveType type, SpecialMoveProcessor proccessor)
	{
		this.type = type;
		this.proccessor = proccessor;
	}
}