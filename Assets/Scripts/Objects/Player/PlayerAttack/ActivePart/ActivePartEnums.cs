using UnityEngine;

public enum ActivePartType
{
	None,
	Basic,
	Test
}

[System.Serializable]
public class ActivePartData
{
	public ActivePartType type;
	[SerializeReference]
	public ActivePartProccessor proccessor;

	public ActivePartData(ActivePartType type, ActivePartProccessor proccessor)
	{
		this.type = type;
		this.proccessor = proccessor;
	}
}