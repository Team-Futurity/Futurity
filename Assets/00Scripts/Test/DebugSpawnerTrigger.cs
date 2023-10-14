using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpawnerTrigger : MonoBehaviour
{
	[SerializeField] private SpawnerManager manager;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			manager.SpawnEnemy();
			gameObject.SetActive(false);
		}
	}
}
