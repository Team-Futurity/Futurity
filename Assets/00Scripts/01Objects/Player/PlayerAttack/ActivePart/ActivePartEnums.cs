using UnityEngine;

public enum SpecialMoveType
{
	None,
	Basic,
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