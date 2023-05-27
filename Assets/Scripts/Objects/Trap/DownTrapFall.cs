using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrapFall : MonoBehaviour
{
	private UnitBase trapUnit;
	private bool isFall = false;
	private List<int> detectObjID = new List<int>();
	private Rigidbody rig;

	private Vector3 startPos;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ground"))
		{
			EndFall();
		}
		else if (other.CompareTag("Monster") || other.CompareTag("Player"))
		{
			var otherObject = other.gameObject;

			foreach (var obj in detectObjID)
			{
				if (otherObject.GetInstanceID() == obj)
				{
					return;
				}

				otherObject.TryGetComponent(out UnitBase objUnit);
				
				objUnit.Hit(trapUnit, 0);
			}
		}
	}

	public void SetPos(Vector3 pos)
	{
		transform.localPosition = pos;
		startPos = pos;
		TryGetComponent(out rig);
	}

	public void SetOwner(UnitBase unit)
	{
		trapUnit = unit;
	}

	public void StartFall()
	{
		if (!isFall)
		{
			isFall = true;

			gameObject.SetActive(isFall);
		}
	}

	public bool GetFalling()
	{
		return isFall;
	}

	private void EndFall()
	{
		if (isFall)
		{
			isFall = false;

			gameObject.SetActive(isFall);
			ResetFall();
		}
	}

	private void ResetFall()
	{
		rig.velocity = Vector3.zero;
		transform.localPosition = startPos;
	}
}