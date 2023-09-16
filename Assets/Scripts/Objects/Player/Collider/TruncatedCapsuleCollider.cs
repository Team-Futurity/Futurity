using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//[RequireComponent(typeof(CapsuleCollider))]
public class TruncatedCapsuleCollider : TruncatedCollider<CapsuleCollider>
{
	// 콜라이더 기본 설정
	public void SetCollider(float angle, float radius)
	{ 
		this.Angle = angle;
		this.Radius = radius;

		truncatedCollider.radius = radius;
	}

	// 원본 콜라이더 내에 들어왔는지 검증하는 메소드
	public override bool IsInCollider(GameObject target)
	{
		Vector3 targetVec = target.transform.position - transform.position;

		return targetVec.magnitude <= Radius;
	}

	// 공격 범위 표시
#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		Gizmos.color = colliderColor;
		var vecs = GetVectorToCut();
		Vector3 leftPos = transform.position + vecs[1];
		Vector3 rightPos = transform.position + vecs[0];

		// Arc
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);
		Handles.DrawSolidArc(transform.position, Vector3.up, vecs[1], Angle, Radius);

		// Line
		if (Angle % 360 == 0) return;
		Gizmos.DrawLine(transform.position, rightPos);
		Gizmos.DrawLine(transform.position, leftPos);
	}
#endif
}
