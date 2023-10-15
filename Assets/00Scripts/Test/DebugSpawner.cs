using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawner : MonoBehaviour
{
	[SerializeField] private List<SpawnerManager> spawnerManagers;

	public void Start()
	{
		spawnerManagers[0].SpawnEnemy();
	}
}
