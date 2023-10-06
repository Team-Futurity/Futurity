using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPartCollider : MonoBehaviour
{
	public bool isOn = false;

	public float radius = .0f;

	public void OnDrawGizmos()
	{
		if (!isOn)
			return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
