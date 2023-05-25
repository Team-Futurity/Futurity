using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteInteractionWindow : MonoBehaviour
{
	[SerializeField]
	private GameObject eliteInteractonWindow;
	[SerializeField]
	private Canvas EnemyCanvas;
	[SerializeField]
	private float detectRadius = 5f;
	private bool playerInRange = false;
	private GameObject currentWindow;

	void Update()
	{
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, detectRadius);
		bool playerDetected = false;
		foreach (var hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.tag == "Player")
			{
				playerDetected = true;
				break;
			}
		}

		if (playerDetected && !playerInRange)
		{
			Vector3 enemyWorldPosition = Camera.main.WorldToScreenPoint(transform.position);
			enemyWorldPosition.x -= Screen.width / 2;
			enemyWorldPosition.y -= Screen.height / 2;
			Debug.Log($"enemyWorldPosition : {enemyWorldPosition}");
			currentWindow = WindowManager.Instance.WindowTopOpen(eliteInteractonWindow, enemyWorldPosition, Vector2.one);

			playerInRange = true;
		}
		else if (!playerDetected && playerInRange)
		{
			playerInRange = false;
			Destroy(currentWindow);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectRadius);
	}
}
