using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TruncatedCapsuleCollider : TruncatedCollider<CapsuleCollider>
{
	public override float Angle
	{
		get { return angle; }
		protected set
		{
			if (value == 360) { angle = 360; }
			else { angle = value % 360; }
		}
	}

	public override float Length { get; protected set; }

	// 콜라이더 기본 설정
	public override void SetCollider(float angle, float radius)
	{ 
		Angle = angle;
		Length = radius;

		ColliderReference.radius = radius;
	}

	// 원본 콜라이더 내에 들어왔는지 검증하는 메소드
	public override bool IsInCollider(GameObject target)
	{
		Vector3 targetVec = target.transform.position - transform.position;

		return targetVec.magnitude <= Length;
	}

	public override void SetColliderActivation(bool isActive)
	{
		ColliderReference.enabled = isActive;
	}

	// 공격 범위 표시
#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		if (ColliderReference == null || !ColliderReference.enabled) { return; }

		Gizmos.color = colliderColor;
		var vecs = GetVectorToCut();

		// Arc
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);
		Handles.DrawSolidArc(transform.position, Vector3.up, vecs[1], Angle, Length);

		// Line
		if (Angle % 360 == 0) return;
		Gizmos.DrawLine(transform.position, vecs[0] + transform.position);
		Gizmos.DrawLine(transform.position, vecs[1] + transform.position);
	}
#endif
}
