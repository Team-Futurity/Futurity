using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Spawn Data", menuName = "ScriptableObject/Spawn Data", order = int.MaxValue)]
public class EnemySpawnData : ScriptableObject
{
	[Header("각 웨이브에서 스폰시킬 수")] [Tooltip("List의 Count가 최대 웨이브 횟수")]
	public List<WaveCount> waveSpawnCounts;
}

[System.Serializable]
public struct WaveCount
{
	public SpawnCount[] wave;
}

[System.Serializable]
public struct SpawnCount
{
	public int meleeCnt;
	public int rangedCnt;
	public int minimalCnt;
}