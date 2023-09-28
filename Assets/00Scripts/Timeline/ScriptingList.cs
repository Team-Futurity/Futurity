using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptingList
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

