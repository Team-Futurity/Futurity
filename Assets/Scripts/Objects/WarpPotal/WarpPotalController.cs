using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPotalController : MonoBehaviour
{
	[SerializeField] private Transform targetPosition;
	[SerializeField] private bool isSceneChanger = false;

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (!isSceneChanger)
			{
				AreaWarpManager.Instance.WarpStart(collision.gameObject, targetPosition);
			} 
		}
	}
}
