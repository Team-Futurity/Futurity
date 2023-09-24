using System;
using UnityEngine;

public class SteageMove : Singleton<SteageMove>
{
	public enum EStageType
	{
		STAGE_2 = 0,
		STAGE_3 = 1
	}

	[SerializeField] private Transform player;
	[SerializeField] private Transform[] spawnPos;

	public void MoveStage(EStageType target)
	{
		player.position = spawnPos[(int)target].position;
	}
}
