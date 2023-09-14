using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private const int MAX_ENEMY_TYPE = 3;
	
	[Header("Component")] 
	[SerializeField] private EnemySpawnData enemySpawnData;
	[SerializeField] private List<GameObject> enemyPrefabs;
	[SerializeField] private Transform enemyParents;
	[SerializeField] private Transform enemyContainer;

	[Header("스폰 상황 확인")] 
	[ReadOnly(false), SerializeField] public int maxWaveCount = 0;
	[ReadOnly(false), SerializeField] private int curWaveIndex = 0;
	[ReadOnly(false), SerializeField] private int[] spawnedEnemyCount;
	public int MaxWaveCount => maxWaveCount;
	public int CurWaveIndex => curWaveIndex;
	public int[] SpawnedEnemyCount => spawnedEnemyCount;
	
	// EnemyPool
	private readonly List<Queue<GameObject>> enemyPool = new List<Queue<GameObject>>();
	private const string MELEE_POS_NAME = "MeleePos";
	private const string RANGED_POS_NAME = "RangedPos";
	private const string MINIMAL_POS_NAME = "MinimalPos";
	
	private void Awake()
	{
		Init();
		CreateEnemyPool();

		for (int i = 0; i < spawnedEnemyCount.Length; ++i)
		{
			Debug.Log(spawnedEnemyCount[i]);
		}
	}
	
	public void EnableEnemyWave()
	{
		if (enemyPool.Count <= 0)
		{
			return;
		}
		
		var pool = enemyPool[0];
		while (pool.Count != 0)
		{
			var enemy = pool.Dequeue();
			
			enemy.transform.SetParent(enemyParents);
			enemy.SetActive(true);
		}

		curWaveIndex++;
		enemyPool.RemoveAt(0);
	}

	public void EnemyDisableEvent()
	{
		
	}
	
	private void CreateEnemyPool()
	{
		int curWaveIndex = 1;
		
		foreach (var spawnData in enemySpawnData.waveSpawnCounts)
		{
			int index = 0;

			for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
			{
				for (int j = 0; j < spawnData.spawnCounts[i]; ++j)
				{
					var enemy = Instantiate(enemyPrefabs[i], enemyContainer);
					enemy.GetComponent<EnemyController>().SpawnInitData(curWaveIndex, EnemyDisableEvent);

					enemy.transform.position = transform.GetChild(curWaveIndex).GetChild(index++).position;
					enemy.transform.forward = Vector3.left;
					enemy.SetActive(false);
					
					enemyPool[curWaveIndex - 1].Enqueue(enemy);
				}
			}

			spawnedEnemyCount[curWaveIndex - 1] = enemyPool[curWaveIndex - 1].Count;
			curWaveIndex++;
		}
	}

	private void Init()
	{
		maxWaveCount = enemySpawnData.waveSpawnCounts.Count;
		spawnedEnemyCount = new int[maxWaveCount];
		
		for (int i = 0; i < maxWaveCount; ++i)
		{
			enemyPool.Add(new Queue<GameObject>());
		}
	}
	
	#region OnlyUseEditor
	public void InstantiatePosition()
	{
		if (enemySpawnData == null)
		{
			return;
		}

		int waveCount = enemySpawnData.waveSpawnCounts.Count;

		for (int i = 0; i < waveCount; ++i)
		{
			var obj = new GameObject("Wave" + (i + 1));
			obj.transform.SetParent(transform);
		}

		int curWave = 1;
		foreach (var spawnData in enemySpawnData.waveSpawnCounts)
		{
			int melee = 0;
			int ranged = 0;
			int minimal = 0;
			
			for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
			{
				switch (i)
				{
					case 0:
						melee = spawnData.spawnCounts[i];
						break;
					
					case 1:
						ranged = spawnData.spawnCounts[i];
						break;
					
					case 2:
						minimal = spawnData.spawnCounts[i];
						break;
				}
			}
			CreateTransformObj(melee, MELEE_POS_NAME, EnemyController.EnemyType.MeleeDefault, curWave);
			CreateTransformObj(ranged, RANGED_POS_NAME, EnemyController.EnemyType.RangedDefault, curWave);
			CreateTransformObj(minimal, MINIMAL_POS_NAME, EnemyController.EnemyType.MinimalDefault, curWave);

			curWave++;
		}
	}

	public void RemoveAllPosition()
	{
		int waveCount = enemySpawnData.waveSpawnCounts.Count;
		
		for (int i = 0; i < waveCount; ++i)
		{
			DestroyImmediate(transform.GetChild(1).gameObject);
		}
	}

	private void CreateTransformObj(int count, string objName, EnemyController.EnemyType type,
		int parentsIndex)

	{
		Color color = Color.clear;
		switch (type)
		{
			case EnemyController.EnemyType.MeleeDefault:
				color = Color.red;
				break;

			case EnemyController.EnemyType.RangedDefault:
				color = Color.green;
				break;

			case EnemyController.EnemyType.MinimalDefault:
				color = Color.yellow;
				break;
		}

		for (int i = 0; i < count; ++i)
		{
			var posObj = new GameObject(objName + (i + 1));

			posObj.transform.SetParent(transform.GetChild(parentsIndex));
			posObj.transform.localPosition = Vector3.zero;

			posObj.AddComponent<DrawPosition>().color = color;
		}
	}

	#endregion

}

