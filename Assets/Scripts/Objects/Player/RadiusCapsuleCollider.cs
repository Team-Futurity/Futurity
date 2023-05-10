using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class RadiusCapsuleCollider : MonoBehaviour 
{
	public float angle;
	public float radius;
	public CapsuleCollider radiusCollider;

	private void Start()
	{
		radiusCollider = GetComponent<CapsuleCollider>();
		radiusCollider.isTrigger = true;
		radiusCollider.enabled = false;
	}

	public void SetCollider(float angle, float radius)
	{ 
		this.angle = angle;
		this.radius = radius;

		radiusCollider.radius = radius;
	}

	public bool IsInCollider(GameObject target)
	{
		float clampedAngle = angle % 360;
		Vector3 targetVec = target.transform.position - transform.position;
		float dot = Vector3.Dot(targetVec.normalized, transform.forward);
		float theta = Mathf.Acos(dot);

		return theta * 2 <= (clampedAngle == 0 && angle > 0 ? 360 : clampedAngle) * Mathf.Deg2Rad;
	}

	public List<GameObject> GetObjectsInCollider(List<GameObject> targets)
	{
		List<GameObject> objects = targets.ToList();
		for(int targetCount = 0; targetCount < objects.Count; targetCount++)
		{ 
			if(!IsInCollider(objects[targetCount]))
			{
				objects.RemoveAt(targetCount);
			}
		}

		return objects;
	}
}
