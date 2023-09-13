using System.Linq;

public class SpawnCountData
{
	public readonly int[] maxSpawnCounts = new int[3];
	public readonly int[] remainingCounts = new int[3];
	public readonly int totalCount = 0;
	public int totalRemainingCount = 0;

	public SpawnCountData(EnemySpawnData data)
	{
		maxSpawnCounts[(int)EnemyController.EnemyType.MeleeDefault] = data.MeleeDefaultSpawnCount;
		maxSpawnCounts[(int)EnemyController.EnemyType.RangedDefault] = data.RangedDefaultSpawnCount;
		maxSpawnCounts[(int)EnemyController.EnemyType.MinimalDefault] = data.MinimalDefaultSpawnCount;
		maxSpawnCounts.CopyTo(remainingCounts, 0);

		totalCount = maxSpawnCounts.Sum();
		totalRemainingCount = remainingCounts.Sum();
	}

	public void UpdateRemainingCounts(int type)
	{
		remainingCounts[type]--;
		totalRemainingCount--;
	}
}