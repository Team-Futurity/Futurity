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
	public enum EMiraeExpression
	{
		NONE,	// Not Change prev expression
		ANGRY,
		IDLE,
		PANIC,
		SHORT_SURPRISE,
		SMILE,
		SURPRISE,
		TRUST_ME
	}
	
	public enum ESariExpression
	{
		NONE,
		ANGRY,
		EMBARRASSED,
		IDLE,
		SURPRISE,
		SAD
	}
	
	public enum EBossExpression
	{
		NONE,
		ANGRY,
		IDLE,
		LAUGH
	}
	public enum ENameType
	{
		MIRAE,
		SONGSARI,
		BOSS,
		SUNKYOUNG
	}

	public EMiraeExpression miraeExpression;
	public ESariExpression sariExpression;
	public EBossExpression bossExpression;
	public ENameType nameType;
	public string scripts;
}

