using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	[Header("컴포넌트")] 
	[SerializeField] private Transform enemyParents;
	
	[Header("스폰 데이터")] 
	[SerializeField] private EnemySpawnData spawnData;
	[HideInInspector] public int curEnemyTotalCount = 0;
	private int totalWaveCount = 0;
	private int curWaveCount = 0;
	private int totalStepCount = 0;
	private int curStepCount = 0;

	[Header("스포너 범위")] 
	[SerializeField] private float spawnRadius = 20.0f;
	[SerializeField] private float yOffset = 0.64f;
	[SerializeField] private Color radiusColor = Color.clear;
	[SerializeField] private int maxCheckCount = 30;
	private int curCheckCount = 0;
	private Transform spawnArea = null;

	[Header("중복 소환 검사 크기")] 
	[SerializeField] private float inspectionRange = 2.0f;

	// Pool Event
	public event GetEnemy GetEnemyEvent;
	public delegate GameObject GetEnemy(EnemyController.EnemyType type);
	
	// 실제 소환 개수 저장
	[HideInInspector] public int[] spawnCount = new int[3];
	private int spawnIndex = 0;

	private void Awake()
	{
		Init();
		GetEnemyEvent += gameObject.GetComponentInParent<SpawnerManager>().GetEnemy;
	}

	public void SpawnEnemy()
	{
		if (curStepCount >= totalStepCount)
		{
			curWaveCount++;
			curStepCount = 0;

			if (curWaveCount >= totalWaveCount)
			{
				Debug.Log("Spawn is Done");
				gameObject.SetActive(false);
				return;
			}
			
			totalStepCount = spawnData.waveSpawnCounts[curWaveCount].wave.Length;
		}

		Debug.Log($"Total Wave : {totalWaveCount} / Cur Wave : {curWaveCount}");
		Debug.Log($"Total Step : {totalStepCount} / Cur Wave : {curStepCount}");
		
		int melee = spawnData.waveSpawnCounts[curWaveCount].wave[curStepCount].meleeCnt;
		int ranged = spawnData.waveSpawnCounts[curWaveCount].wave[curStepCount].rangedCnt;
		int minimal = spawnData.waveSpawnCounts[curWaveCount].wave[curStepCount].minimalCnt;
		curEnemyTotalCount = melee + ranged + minimal;
		
		Debug.Log(curEnemyTotalCount);
		spawnIndex = 0;
		
		PlaceEnemy(melee, EnemyController.EnemyType.MeleeDefault);
		PlaceEnemy(ranged, EnemyController.EnemyType.RangedDefault);
		PlaceEnemy(minimal, EnemyController.EnemyType.MinimalDefault);
	}

	public void OnDisableEvent()
	{
		curEnemyTotalCount--;
		Debug.Log($"Remain : {curEnemyTotalCount}");

		if (curEnemyTotalCount <= 0)
		{
			curStepCount++;
			SpawnEnemy();
		}
	}

	public int[] GetTotalCreateCount()
	{
		int[] result = new int[3];

		foreach (var totalWave in spawnData.waveSpawnCounts)
		{
			foreach (var count in totalWave.wave)
			{
				result[0] += count.meleeCnt;
				result[1] += count.rangedCnt;
				result[2] += count.minimalCnt;
			}
		}
		
		return result;
	}
	
	private void PlaceEnemy(int count, EnemyController.EnemyType type)
	{
		for (int i = 0; i < count; ++i)
		{
			Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
			Vector3 spawnPos = new Vector3(randomPos.x, yOffset, randomPos.y) + spawnArea.position;

			Collider[] colliders = Physics.OverlapSphere(spawnPos, inspectionRange);
			bool isEnemyFound = false;
			
			foreach (Collider col in colliders)
			{
				if (col.CompareTag("Enemy"))
				{
					isEnemyFound = true;
					curCheckCount++;
					break;
				}
			}

			if (isEnemyFound == true && curCheckCount <= maxCheckCount)
			{
				i--;
				continue;
			}
			
			curCheckCount = 0;

			var enemy = GetEnemyEvent?.Invoke(type);
			if (enemy == null)
			{
				return;
			}
			
			enemy.GetComponent<EnemyController>().RegisterEvent(OnDisableEvent);
			enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.Euler(0, -90f, 0));
			enemy.transform.SetParent(enemyParents);
			enemy.SetActive(true);

			spawnCount[spawnIndex]++;
		}

		spawnIndex++;
	}
	
	private void Init()
	{
		totalWaveCount = spawnData.waveSpawnCounts.Count;
		totalStepCount = spawnData.waveSpawnCounts[curWaveCount].wave.Length;

		spawnArea = transform;
	}

	#region Editor
	private void OnDrawGizmos()
	{
		Handles.color = radiusColor;
		Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, spawnRadius);
	}
	#endregion
}

