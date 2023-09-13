using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private const int MAX_ENEMY_TYPE = 3;
	
	[Header("Component")] 
	[SerializeField] private EnemySpawnData enemySpawnData;
	[SerializeField] private List<GameObject> enemyPrefabs;
	[SerializeField] private List<Transform> positionParents;
	[SerializeField] private Transform enemyParents;
	[SerializeField] private Transform enemyContainer;
	public SpawnCountData data;
	
	// EnemyPool
	private readonly List<Queue<GameObject>> enemyPool = new List<Queue<GameObject>>();

	private const string MELEE_POS_NAME = "MeleePos";
	private const string RANGED_POS_NAME = "RangedPos";
	private const string MINIMAL_POS_NAME = "MinimalPos";
	
	private void Awake()
	{
		Init();
		CreateEnemyPool();

		for (int i = 0; i < enemySpawnData.firstSpawnCount.Length; ++i)
		{
			SpawnEnemy(enemySpawnData.firstSpawnCount[i], i);
		}

		Debug.Log(
			$"{data.totalCount}   /   {data.totalRemainingCount} = {enemyPool[0].Count} + {enemyPool[1].Count} + {enemyPool[2].Count}");
	}
	
	public void SpawnEnemy(int spawnCount, int type)
	{
		if (spawnCount > enemyPool[type].Count)
		{
			Debug.Log("Spawn Count is to Large!!");
			return;
		}

		for (int i = 0; i < spawnCount; ++i)
		{
			var enemy = enemyPool[type].Dequeue();
			
			enemy.transform.SetParent(enemyParents);
			enemy.SetActive(true);

			data.UpdateRemainingCounts(type);
		}
	}
	
	private void CreateEnemyPool()
	{
		for (int i = 0; i < MAX_ENEMY_TYPE; ++i)
		{
			var prefabs = enemyPrefabs[i];
			
			for (int j = 0; j < data.maxSpawnCounts[i]; ++j)
			{
				var enemy = Instantiate(prefabs, enemyContainer);

				enemy.transform.position = positionParents[i].GetChild(j).position;
				enemy.transform.forward = Vector3.left;
				enemy.SetActive(false);
				
				enemyPool[i].Enqueue(enemy);
			}
		}
	}

	private void Init()
	{
		data = new SpawnCountData(enemySpawnData);
		
		enemyPool.Add(new Queue<GameObject>());
		enemyPool.Add(new Queue<GameObject>());
		enemyPool.Add(new Queue<GameObject>());
	}
	
	#region OnlyUseEditor
	public void InstantiatePosition()
	{
		if (enemySpawnData == null)
		{
			return;
		}
		
		int meleeCount = enemySpawnData.MeleeDefaultSpawnCount;
		int rangeCount = enemySpawnData.RangedDefaultSpawnCount;
		int minimalCount = enemySpawnData.MinimalDefaultSpawnCount;
		
		CreateTransformObj(meleeCount, MELEE_POS_NAME, EnemyController.EnemyType.MeleeDefault);
		CreateTransformObj(rangeCount, RANGED_POS_NAME, EnemyController.EnemyType.RangedDefault);
		CreateTransformObj(minimalCount, MINIMAL_POS_NAME, EnemyController.EnemyType.MinimalDefault);
	}

	public void RemoveAllPosition()
	{
		for (int i = 0; i < positionParents.Count; ++i)
		{
			int childCount = positionParents[i].childCount;
			for (int j = 0; j < childCount; ++j)
			{
				DestroyImmediate(positionParents[i].GetChild(0).gameObject);
			}
		}
	}
	
	private void CreateTransformObj(int count, string objName, EnemyController.EnemyType type)
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
			
			posObj.transform.SetParent(positionParents[(int)type]);
			posObj.transform.localPosition = Vector3.zero;

			posObj.AddComponent<DrawPosition>().color = color;
		}
	}
	#endregion

}

