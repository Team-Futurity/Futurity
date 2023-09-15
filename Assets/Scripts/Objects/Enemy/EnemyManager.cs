using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private const int MAX_ENEMY_TYPE = 3;
	
	[Header("컴포넌트")] 
	[SerializeField] private List<EnemySpawner> spawnerList;
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private Transform enemyContainer;

	[Header("Enemy Pool")] 
	[SerializeField] private int initCount = 20;
	private List<Queue<GameObject>> enemyPool = new List<Queue<GameObject>>();

	private void Awake()
	{
		for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
		{
			enemyPool.Add(new Queue<GameObject>());
		}
		
		CreateEnemyObject(initCount, EnemyController.EnemyType.MeleeDefault);
		CreateEnemyObject(initCount, EnemyController.EnemyType.RangedDefault);
		CreateEnemyObject(initCount, EnemyController.EnemyType.MinimalDefault);
	}

	public GameObject GetEnemy(EnemyController.EnemyType type)
	{
		if (enemyPool[(int)type].Count <= 0)
		{
			CreateEnemyObject(initCount / 2, type);
		}
		
		GameObject enemy = enemyPool[(int)type].Dequeue();
		return enemy;
	}

	// 체크용 
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			spawnerList[0].SpawnEnemy();
		}
	}

	private void CreateEnemyObject(int count, EnemyController.EnemyType type)
	{
		int index = (int)type;

		for (int i = 0; i < count; ++i)
		{
			var enemy = Instantiate(enemyPrefabs[index], enemyContainer);
			enemy.SetActive(false);
			
			enemyPool[index].Enqueue(enemy);
		}
	}
}
