using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
	private const int MAX_ENEMY_TYPE = 3;
	
	[Header("컴포넌트")] 
	[SerializeField] private List<EnemySpawner> spawnerList;
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private Transform enemyContainer;

	[Header("Debug 패널")] 
	[Tooltip("다음 웨이브 조건"), SerializeField] private int nextWaveCondition = 3;
	[ReadOnly(false), SerializeField] private int[] totalSpawnCount;
	[ReadOnly(false), SerializeField] private int curWaveSpawnCount;
	
	private readonly List<Queue<GameObject>> enemyPool = new List<Queue<GameObject>>();

	private void Awake()
	{
		for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
		{
			enemyPool.Add(new Queue<GameObject>());
		}

		totalSpawnCount = new int[3];
		InitSpawnerData();
		
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.MeleeDefault], EnemyController.EnemyType.MeleeDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.RangedDefault], EnemyController.EnemyType.RangedDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.MinimalDefault], EnemyController.EnemyType.MinimalDefault);
	}

	private void Start()
	{
		foreach (var spawner in spawnerList)
		{
			spawner.SpawnEnemy();
			curWaveSpawnCount += spawner.GetCurrentSpawnCount();
		}
	}

	public GameObject GetEnemy(EnemyController.EnemyType type)
	{
		if (enemyPool[(int)type].Count <= 0)
		{
			CreateEnemyObject(1, type);
		}
		
		GameObject enemy = enemyPool[(int)type].Dequeue();
		return enemy;
	}

	private void InitSpawnerData()
	{
		foreach (var spawner in spawnerList)
		{
			int[] arr = spawner.GetTotalCreateCount();
			spawner.GetComponent<EnemySpawner>().spawnerDisableEvent.AddListener(SpawnerDisableEvent);

			for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
			{
				totalSpawnCount[i] += arr[i];
			}
		}
	}

	private void CreateEnemyObject(int count, EnemyController.EnemyType type)
	{
		int index = (int)type;

		for (int i = 0; i < count; ++i)
		{
			var enemy = Instantiate(enemyPrefabs[index], enemyContainer);
			enemy.GetComponent<EnemyController>().disableEvent.AddListener(MonsterDisableEvent);
			enemy.SetActive(false);
			
			enemyPool[index].Enqueue(enemy);
		}
	}

	private void MonsterDisableEvent()
	{
		curWaveSpawnCount--;

		if (curWaveSpawnCount > nextWaveCondition)
		{
			return;
		}

		foreach (var spawner in spawnerList)
		{
			spawner.SpawnEnemy();
			curWaveSpawnCount += spawner.GetCurrentSpawnCount();
		}
	}
	
	private void SpawnerDisableEvent(EnemySpawner spawner)
	{
		spawnerList.Remove(spawner);
	}
}
