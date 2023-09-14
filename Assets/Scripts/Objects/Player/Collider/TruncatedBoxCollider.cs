using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class TruncatedBoxCollider : TruncatedCollider<BoxCollider>
{
	public Vector2 offset;
	public Vector2 size;

	// 콜라이더 기본 설정
	public void SetCollider(float angle, float radius, Vector2 offset, Vector2 size)
	{
		Angle = angle;
		Radius = radius;

		this.offset = offset;
		this.size = size;

		truncatedCollider.center = new Vector3(offset.x, truncatedCollider.center.y, offset.y);
		truncatedCollider.size = new Vector3(size.x, truncatedCollider.size.y, size.y);
	}

	public override bool IsInCollider(GameObject target)
	{
		Vector3 targetVec = target.transform.position - truncatedCollider.center;

		float absVecX = Mathf.Abs(targetVec.x);
		float absVecZ = Mathf.Abs(targetVec.z);
		float halfSizeX = size.x * 0.5f;
		float halfSizeZ = size.y * 0.5f;

		return absVecX <= halfSizeX && absVecZ <= halfSizeZ;
	}


	// 0 : leftTop
	// 1 : leftBottom
	// 2 : rightTop
	// 3 : rightBottom
	private Vector2[] GetRectanglePoints()
	{
		Vector2[] points = new Vector2[4];
		Vector2 originPos = new Vector2(transform.position.x, transform.position.z);
		Vector2 finalPos = offset + originPos;

		float halfSizeX = size.x * 0.5f;
		float halfSizeZ = size.y * 0.5f;

		points[0] = new Vector2(-halfSizeX, halfSizeZ);
		points[1] = new Vector2(-halfSizeX, -halfSizeZ);
		points[2] = new Vector2(halfSizeX, halfSizeZ);
		points[3] = new Vector2(halfSizeX, -halfSizeZ);

		for(int i = 0; i < 4; i++)
		{
			points[i] += finalPos;
		}

		return points;
	}

#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		Gizmos.color = colliderColor;
		var vecs = GetVectorToCut();
		var rectanglePoints = GetRectanglePoints();

		Vector3 leftPos = transform.position + vecs[1];
		Vector3 rightPos = transform.position + vecs[0];

		// Head(TopLine)
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);
		Handles.DrawLines()

		// Line
		if (Angle % 360 == 0) return;
		Gizmos.DrawLine(transform.position, rightPos);
		Gizmos.DrawLine(transform.position, leftPos);
	}
#endif
}
