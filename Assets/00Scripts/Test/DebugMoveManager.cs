using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMoveManager : MonoBehaviour
{
	[SerializeField] private Transform[] movePosition;
	[SerializeField] private SpawnerManager[] spawnerManagers;
	private Transform player;

	private void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			MovePlayer(0);
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			MovePlayer(1);
			spawnerManagers[0].SpawnEnemy();
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			MovePlayer(2);
			spawnerManagers[1].SpawnEnemy();
		}

		if (Input.GetKeyDown(KeyCode.F4))
		{
			SceneLoader.Instance.LoadScene(ChapterSceneName.BOSS_CHAPTER);
		}
	}

	private void MovePlayer(int index)
	{
		Vector3 position = movePosition[index].position;
		Quaternion rotation = movePosition[index].rotation;
		player.SetPositionAndRotation(position, rotation);
	}
}
