using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerManager : MonoBehaviour
{
	public readonly static int MAX_ENEMY_TYPE = 4;
	
	[Header("컴포넌트")] 
	[SerializeField] private List<EnemySpawner> spawnerList;
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private Transform enemyContainer;

	[Header("Event")] 
	[SerializeField] private EAreaType areaType;
	[SerializeField] private UnityEvent<SpawnerManager, EAreaType> spawnEndEvent;
	[SerializeField] private UnityEvent<SpawnerManager, EAreaType> interimEvent;
	[HideInInspector] public bool isEventEnable = false;

	[Header("Debug 패널")] 
	[Tooltip("다음 웨이브 조건"), SerializeField] private int nextWaveCondition = 3;
	[SerializeField] private int curWaveSpawnCount = 0;
	[ReadOnly(false), SerializeField] private int[] totalSpawnCount;
	
	private readonly List<Queue<GameObject>> enemyPool = new List<Queue<GameObject>>();

	// Property
	public int CurWaveSpawnCount => curWaveSpawnCount;
	public int SpawnerListCount => spawnerList.Count;
	
	private void Awake()
	{
		for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
		{
			enemyPool.Add(new Queue<GameObject>());
		}

		totalSpawnCount = new int[MAX_ENEMY_TYPE];
		InitSpawnerData();
		
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.MeleeDefault], EnemyController.EnemyType.MeleeDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.RangedDefault], EnemyController.EnemyType.RangedDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.MinimalDefault], EnemyController.EnemyType.MinimalDefault);
		CreateEnemyObject(totalSpawnCount[(int)EnemyController.EnemyType.EliteDefault], EnemyController.EnemyType.EliteDefault);
	}

	public void SpawnEnemy()
	{
		foreach (var spawner in spawnerList)
		{
			spawner.SpawnEnemy();
			curWaveSpawnCount += spawner.GetCurrentSpawnCount();
		}
		
		UpdateSpawnerList();
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
			enemy.GetComponent<EnemyController>().RegisterEvent(MonsterDisableEvent);
			enemy.SetActive(false);
			
			enemyPool[index].Enqueue(enemy);
		}
	}

	private void MonsterDisableEvent()
	{
		curWaveSpawnCount--;
	
		spawnEndEvent?.Invoke(this, areaType);
		interimEvent?.Invoke(this, areaType);
		
		if (curWaveSpawnCount > nextWaveCondition || spawnerList.Count <= 0)
		{
			return;
		}

		foreach (EnemySpawner spawner in spawnerList)
		{
			spawner.SpawnEnemy();
			curWaveSpawnCount += spawner.GetCurrentSpawnCount();
		}
		
		UpdateSpawnerList();
	}

	private void UpdateSpawnerList()
	{
		List<int> removeIndex = new List<int>();

		for (int i = spawnerList.Count - 1; i >= 0; --i)
		{
			if (spawnerList[i].IsSpawnEnd() == true)
			{
				removeIndex.Add(i);
			}
		}

		foreach (int index in removeIndex)
		{
			spawnerList[index].gameObject.SetActive(false);		
		}
	}
	
	private void SpawnerDisableEvent(EnemySpawner spawner)
	{
		spawnerList.Remove(spawner);
	}
}
