using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class RadiusCapsuleCollider : MonoBehaviour 
{
	public float angle;
	public float radius;
	public CapsuleCollider radiusCollider;
	[SerializeField] private Color colliderColor = Color.red;

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

		var vecs = GetRadiusVector();

		Vector3 leftPos = transform.position + vecs[1];
		Vector3 rightPos = transform.position + vecs[0];

		Ray leftRay = new Ray(transform.position, (leftPos - transform.position).normalized);
		Ray rightRay = new Ray(transform.position, (rightPos - transform.position).normalized);

		var leftRayCaster = Physics.Raycast(leftRay, radius);
		var rightRayCaster = Physics.Raycast(rightRay, radius);

		if(leftRayCaster || rightRayCaster)
		{
			return true;
		}

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

	public Vector3[] GetRadiusVector()
	{
		Vector3[] vecs = new Vector3[2];
		float clampedAngle = angle % 360 * 0.5f;
		float posX, posY, posZ;
		float theta = 90 - transform.eulerAngles.y;

		posY = 0;

		// right
		posX = Mathf.Cos((theta - clampedAngle) * Mathf.Deg2Rad) * radius;
		posZ = Mathf.Sin((theta - clampedAngle) * Mathf.Deg2Rad) * radius;
		vecs[0] = new Vector3(posX, posY, posZ);

		// left
		posX = Mathf.Cos((theta + clampedAngle) * Mathf.Deg2Rad) * radius;
		posZ = Mathf.Sin((theta + clampedAngle) * Mathf.Deg2Rad) * radius;
		vecs[1] = new Vector3(posX, posY, posZ);

		return vecs;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = colliderColor;
		var vecs = GetRadiusVector();
		Vector3 leftPos = transform.position + vecs[1];
		Vector3 rightPos = transform.position + vecs[0];

		// Arc
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);
		Handles.DrawSolidArc(transform.position, Vector3.up, vecs[1], angle, radius);

		// Line
		if (angle % 360 == 0) return;
		Gizmos.DrawLine(transform.position, rightPos);
		Gizmos.DrawLine(transform.position, leftPos);
	}
#endif
}
