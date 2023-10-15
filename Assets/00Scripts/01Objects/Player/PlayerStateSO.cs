using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateCondition
{
	public PlayerState stateType;
	public bool isWorkable = true;
}

[CreateAssetMenu(fileName = "PlayerState", menuName = "ScriptableObject/Player/PlayerState")]
public class PlayerStateSO : ScriptableObject
{
	public List<PlayerStateCondition> conditions;
}
