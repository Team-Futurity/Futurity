using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveZone : MonoBehaviour
{
	[SerializeField] private float yOffset = 100.0f;

	private GameObject player;
	private List<GameObject> enemies;
	private Vector3 originPlayerTf;
	private readonly List<Vector3> originEnemiesTf = new List<Vector3>();

	public void SetActiveZone(GameObject playerObj, List<GameObject> enemy)
	{
		this.player = playerObj;
		enemies = enemy;

		gameObject.SetActive(true);
		
		// Player and ActiveZone position Offset setting
		Vector3 playerPos = playerObj.transform.position;
		transform.position = new Vector3(playerPos.x, yOffset, playerPos.z);
		originPlayerTf = playerPos;
		playerObj.transform.position = ChangePosition(playerPos);
		
		// Set Enemy Y Offset
		if (enemy.Count == 0)
		{
			return;
		}
		
		foreach (GameObject obj in enemy)
		{
			originEnemiesTf.Add(obj.transform.position);
			obj.transform.position = ChangePosition(obj.transform.position);
		}
	}

	public void DisableActiveZone()
	{
		player.transform.position = originPlayerTf;
		int index = 0;
		
		
		foreach (Vector3 originTf in originEnemiesTf)
		{
			enemies[index++].transform.position = originTf;
		}
		
		enemies.Clear();
		originEnemiesTf.Clear();
		originPlayerTf = Vector3.zero;
		
		gameObject.SetActive(false);
	}

	private Vector3 ChangePosition(Vector3 tf) => new Vector3(tf.x, yOffset, tf.y);
}
