using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	private const int MAX_ENEMY_TYPE = 3;

	[Header("컴포넌트")] 
	[SerializeField] private Transform enemyParents;
	
	[Header("스폰 데이터")] 
	[SerializeField] private EnemySpawnData spawnData;
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
	
	// 실제 소환 개수 저장
	[HideInInspector] public int[] spawnCount = new int[3];
	private int spawnIndex = 0;

	private void Awake()
	{
		Init();
	}

	public void SpawnEnemy(GameObject[] enemyPrefabs)
	{
		if (curWaveCount >= totalWaveCount)
		{
			return;
		}
		
		if (curStepCount >= totalStepCount)
		{
			curWaveCount++;
			curStepCount = 0;
		}
		
		int melee = spawnData.waveSpawnCounts[curWaveCount].wave[curStepCount].meleeCnt;
		int ranged = spawnData.waveSpawnCounts[curWaveCount].wave[curStepCount].rangedCnt;
		int minimal = spawnData.waveSpawnCounts[curWaveCount].wave[curStepCount].minimalCnt;

		spawnIndex = 0;
		CreateEnemy(melee, enemyPrefabs[0]);
		CreateEnemy(ranged, enemyPrefabs[1]);
		CreateEnemy(minimal, enemyPrefabs[2]);
	}

	private void CreateEnemy(int count, GameObject prefab)
	{
		for (int i = 0; i < count; ++i)
		{
			Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
			Vector3 spawnPos = new Vector3(randomPos.x, yOffset, randomPos.y) + spawnArea.position;

			Collider[] colliders = Physics.OverlapSphere(spawnPos, inspectionRange);
			bool isEnemyFound = false;
			foreach (var col in colliders)
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

			// TODO : 풀링을 사용한 방법으로 변경
			curCheckCount = 0;
			
			var enemy = Instantiate(prefab, spawnPos, Quaternion.Euler(0, -90f, 0));
			enemy.transform.SetParent(enemyParents);

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
	
	private void OnDrawGizmos()
	{
		Handles.color = radiusColor;
		Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, spawnRadius);
	}
}

