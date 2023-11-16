using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	[Header("컴포넌트")] 
	[SerializeField] private Transform enemyParents;

	[Header("스폰 데이터")] 
	[SerializeField] private EnemySpawnData spawnData;
	[SerializeField] private float yRotation = -90.0f;
	private int totalWaveCount = 0;
	private int curWaveCount = 0;
	
	[Header("스포너 범위")] 
	[SerializeField] private float spawnRadius = 20.0f;
	[SerializeField] private float yOffset = 0.64f;
	[SerializeField] private Color radiusColor = Color.clear;
	[SerializeField] private int maxCheckCount = 30;
	private int curCheckCount = 0;
	private Transform spawnArea = null;

	[Header("중복 소환 검사 크기")] 
	[SerializeField] private float inspectionRange = 2.0f;

	// Event
	public event GetEnemy GetEnemyEvent;
	public delegate GameObject GetEnemy(EnemyType type);
	[HideInInspector] public UnityEvent<EnemySpawner> spawnerDisableEvent;
	
	// 실제 소환 개수 저장
	[ReadOnly(false)] public int[] curWaveEnemyCount = new int[SpawnerManager.MAX_ENEMY_TYPE];
	
	// Enemy Type Array
	private EnemyType[] enemyTypes = 
	{
		EnemyType.M_CF, EnemyType.D_LF, EnemyType.T_DF, EnemyType.E_DF, EnemyType.D_BF,
		EnemyType.M_JF
	};

	private void Awake()
	{
		Init();
		GetEnemyEvent += gameObject.GetComponentInParent<SpawnerManager>().GetEnemy;
	}

	public void SpawnEnemy()
	{
		ClearCurWaveSpawnCounts();
		int[] curWave = GetCurrentWaveSpawnCount(curWaveCount);

		for (int i = 0; i < curWave.Length; ++i)
		{
			PlaceEnemy(curWave[i], enemyTypes[i]);
		}
		
		curWaveCount++;
	}
	
	public int[] GetTotalCreateCount()
	{
		int[] result = new int[SpawnerManager.MAX_ENEMY_TYPE];

		foreach (SpawnCount data in spawnData.waveSpawnCounts)
		{
			for (int i = 0; i < SpawnerManager.MAX_ENEMY_TYPE; ++i)
			{
				result[i] += data.spawnCount[i];
			}
		}
		
		return result;
	}

	public int GetCurrentSpawnCount() => (curWaveEnemyCount.Sum());
	public bool IsSpawnEnd() => (curWaveCount >= totalWaveCount);

	private void PlaceEnemy(int count, EnemyType type)
	{
		if (count <= 0)
		{
			return;
		}

		for (int i = 0; i < count; ++i)
		{
			Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
			Vector3 spawnPos = new Vector3(randomPos.x, 0, randomPos.y) + spawnArea.position;
			spawnPos.y = yOffset;

			Collider[] colliders = Physics.OverlapSphere(spawnPos, inspectionRange);

			bool isEnemyFound = colliders.Any(col => col.CompareTag("Enemy"));
			curCheckCount = (isEnemyFound) ? curCheckCount++ : curCheckCount;

			if (isEnemyFound == true && curCheckCount <= maxCheckCount)
			{
				i--;
				continue;
			}

			curCheckCount = 0;

			GameObject enemy = GetEnemyEvent?.Invoke(type);
			if (enemy == null)
			{
				return;
			}

			enemy.transform.SetPositionAndRotation(spawnPos, Quaternion.Euler(0, yRotation, 0));
			enemy.transform.SetParent(enemyParents);
			enemy.SetActive(true);

			curWaveEnemyCount[(int)type]++;
		}
	}

	private void Init()
	{
		totalWaveCount = spawnData.waveSpawnCounts.Count;
		spawnArea = transform;
	}

	private void OnDisable()
	{
		spawnerDisableEvent?.Invoke(this);
	}

	private void ClearCurWaveSpawnCounts()
	{
		for (int i = 0; i < curWaveEnemyCount.Length; ++i)
		{
			curWaveEnemyCount[i] = 0;
		}
	}

	private int[] GetCurrentWaveSpawnCount(int index) => spawnData.waveSpawnCounts[index].spawnCount;

	#region Editor
	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = radiusColor;
		Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, spawnRadius);
	}
	#endif
	#endregion
}

