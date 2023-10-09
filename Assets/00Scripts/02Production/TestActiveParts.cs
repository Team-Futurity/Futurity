using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActiveParts : MonoBehaviour
{
	[SerializeField] private ActiveZone activeZone;
	[SerializeField] private GameObject playerTf;

	private const int MAX_COUNT = 50;
	private List<GameObject> enemies = new List<GameObject>();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F8))
		{
			TestActive();
			activeZone.SetActiveZone(playerTf, enemies);
		}

		if (Input.GetKeyDown(KeyCode.F9))
		{
			activeZone.DisableActiveZone();
		}
	}

	private void TestActive()
	{
		enemies.Clear();

		Collider[] colliders = new Collider[MAX_COUNT];
		int count = Physics.OverlapSphereNonAlloc(transform.position, 20.0f, colliders);

		for (int i = 0; i < count; ++i)
		{
			if (colliders[i].gameObject.CompareTag("Enemy"))
			{
				enemies.Add(colliders[i].gameObject);
			}
		}
	}
}
