using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Timeline Scripts", menuName = "ScriptableObject/Timeline Scripts", order = int.MaxValue)]
public class ScriptingList : ScriptableObject
{
	public List<ScriptingStruct> scriptList;
}

[Serializable]
public struct ScriptingStruct
{
	public enum EExpressionType
	{
		NONE,	// Not Change prev expression
		ANGRY,
		NORMAL,
		PANIC,
		SMILE,
		SURPRISE,
		TRUST_ME
	}

	public EExpressionType expressionType;
	public string name;
	public string scripts;
}

