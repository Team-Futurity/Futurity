using System.Collections;
using System.Collections.Generic;
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

	protected override void OnDrawGizmos()
	{
		throw new System.NotImplementedException();
	}
}
