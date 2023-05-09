using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPotalController : MonoBehaviour
{
	[SerializeField] private Transform targetPosition;

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("Player"))
		{
			AreaWarpManager.Instance.WarpStart(collision.gameObject, targetPosition);
		}
	}
}
