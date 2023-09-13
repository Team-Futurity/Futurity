using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Spawn Data", menuName = "ScriptableObject/Spawn Data", order = int.MaxValue)]
public class EnemySpawnData : ScriptableObject
{
	[field: Header("스폰시킬 적 숫자")]
	[field: SerializeField] public int MeleeDefaultSpawnCount { get; private set; }
	[field: SerializeField] public int RangedDefaultSpawnCount { get; private set; }
	[field: SerializeField] public int MinimalDefaultSpawnCount { get; private set; }

	[Header("처음 소환시킬 적 숫자")] 
	[Tooltip("스포터가 Enable 시 바로 소환할 적 숫자를 저장")] public int[] firstSpawnCount = new int[3];
}
