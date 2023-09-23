using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

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

#if UNITY_EDITOR
	#region EffortForCutTheRectangle
	/*
	private Vector2[] GetFinalPoints(int pointCount, Vector2 originPos, Vector2[] rectanglePoints, Vector2 leftPoint, Vector2 rightPoint)
	{
		List<Vector2> points = new List<Vector2>();
		
		if(pointCount == 3 || pointCount == 5)
		{
			points.Add(leftPoint);
		}

		if(pointCount == 4 || pointCount == 5)
		{
			points.Add(rectanglePoints[0]);
			points.Add(rectanglePoints[2]);
		}

		if (pointCount == 4)
		{
			points.Add(rectanglePoints[1]);
			points.Add(rectanglePoints[3]);
		}

		if (pointCount == 3 || pointCount == 5)
		{
			if (leftPoint != rightPoint) { points.Add(rightPoint); }
			
			points.Add(originPos);
		}

		return points.ToArray();
	}

	private Vector3[] Vector2ToVector3(Vector2[] vec2s)
	{
		Vector3[] vec3s = new Vector3[vec2s.Length];

		for(int i = 0; i <  vec2s.Length; i++)
		{
			vec3s[i] = new Vector3(vec2s[i].x, 0, vec2s[i].y);
		}

		return vec3s;
	}

	protected override void OnDrawGizmos()
	{
		*//*Gizmos.color = colliderColor;
		var vecs = GetVectorToCut();
		var rectanglePoints = GetRectanglePoints();

		Vector3 vec3Offset = new Vector3(offset.x, 0, offset.y);
		Vector3 originPos = transform.position + vec3Offset;
		originPos.y = 0;
		originPos.z -= size.y * 0.5f;

		Vector3 leftPos = originPos + vecs[1];
		Vector3 rightPos = originPos + vecs[0];

		Debug.Log(Vector3.Dot(leftPos, Vector3.forward) * Mathf.Rad2Deg);

		Vector2 leftIntersectionPoint; 
		Vector2 rightIntersectionPoint; 
		int leftPoints = GetintersectionPoint(rectanglePoints[0], rectanglePoints[2], rectanglePoints[0], rectanglePoints[1], leftPos, out leftIntersectionPoint);
		int rightPoints = GetintersectionPoint(rectanglePoints[0], rectanglePoints[2], rectanglePoints[2], rectanglePoints[3], rightPos, out rightIntersectionPoint);

		if(leftPoints != rightPoints) { FDebug.LogWarning("[TruncatedBoxCollider] Collider is Unbalanced."); return; }

		Vector2[] finalPoints = GetFinalPoints(leftPoints, originPos, rectanglePoints, leftIntersectionPoint, rightIntersectionPoint);
		Vector3[] points = Vector2ToVector3(finalPoints);
		//points[0] = leftPos; points[3] = rightPos;
		// Draw Lines
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);
		for (int i = 0; i < points.Length - 1; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
		Gizmos.DrawLine(points[points.Length - 1], points[0]);
		*//*Handles.DrawPolyLine(Vector2ToVector3(finalPoints));*//*

	Gizmos.color = colliderColor;
		Vector3 origin = transform.position + new Vector3(offset.x, 0, offset.y);
		Vector2 originVec2 = new Vector2(origin.x, origin.z);
		float originYPos = transform.position.y;
		var vecs = GetVectorToCut();
		var rectanglePoints = GetRectanglePoints(origin, new Vector2(transform.forward.x, transform.forward.z));

		Vector3 leftLine = vecs[1] + origin;
		Vector3 rightLine = vecs[0] + origin;

		// 선 색 세팅
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);

		// 상판
		Vector2[] intersectionPoint1, intersectionPoint2;
		int dir = GetintersectionPoint(rectanglePoints, originVec2, new Vector2(leftLine.x, leftLine.z), out intersectionPoint1);
		GetintersectionPoint(rectanglePoints, originVec2, new Vector2(rightLine.x, rightLine.z), out intersectionPoint2);

		if (dir >= 0)
		{
			Gizmos.DrawLine(new Vector3(intersectionPoint1[0].x, originYPos, intersectionPoint1[0].y), new Vector3(intersectionPoint2[0].x, originYPos, intersectionPoint2[0].y));
		}

		// 사이드
		if (dir >= 1)
		{
			Gizmos.DrawLine(new Vector3(rectanglePoints[2].x, originYPos, rectanglePoints[2].y), new Vector3(intersectionPoint2[1].x, originYPos, intersectionPoint2[1].y));
			Gizmos.DrawLine(new Vector3(rectanglePoints[0].x, originYPos, rectanglePoints[0].y), new Vector3(intersectionPoint1[1].x, originYPos, intersectionPoint1[1].y));
		}

		// 범위 선
		Vector2 point1 = intersectionPoint1[intersectionPoint1.Length - 1];
		Vector2 point2 = intersectionPoint2[intersectionPoint2.Length - 1];
		Gizmos.DrawLine(origin, new Vector3(point1.x, originYPos, point1.y));
		Gizmos.DrawLine(origin, new Vector3(point2.x, originYPos, point2.y));


		*//*if (dir >= 0)
		{
			Gizmos.DrawLine(new Vector3(intersectionPoint1.x, transform.position.y, intersectionPoint1.y), new Vector3(intersectionPoint2.x, transform.position.y, intersectionPoint2.y));
		}*//*

		// 좌우

		// 하단
	}*/
	#endregion

	// 0 : leftTop
	// 1 : leftBottom
	// 2 : rightBottom
	// 3 : rightTop
	private Vector2[] GetRectanglePoints(Vector2 origin, Vector2 forward)
	{
		Vector2[] points = new Vector2[4];

		float halfSizeX = size.x * 0.5f;
		float halfSizeZ = size.y * 0.5f;

		points[0] = new Vector2(-halfSizeX, halfSizeZ);
		points[1] = new Vector2(-halfSizeX, -halfSizeZ);
		points[2] = new Vector2(halfSizeX, -halfSizeZ);
		points[3] = new Vector2(halfSizeX, halfSizeZ);

		for (int i = 0; i < 4; i++)
		{
			points[i] += origin;
			points[i].RotateToDirection(forward);
		}

		return points;
	}

	private bool IsInRectangle(Vector2 point, Vector2 rectangleCenter, Vector2[] rectanglePoints)
	{
		float halfBoxSizeX = (rectanglePoints[0] - rectanglePoints[3]).x;

		double topCross = point.Cross(rectanglePoints[3] - rectanglePoints[0]);
		double leftCross = point.Cross(rectanglePoints[0] - rectanglePoints[1]);

		return topCross < Mathf.Epsilon && leftCross < Mathf.Epsilon;
	}

	// 점을 모두 저장하고 충돌한 면이 어딘지를 반환함
	// 0 : top, 1 : side, 2 : bottom
	private void GetIntersectionPoint(Vector2[] rectanglePoints, Vector2 origin, Vector2 line, out Vector2 intersectionPoint)
	{
		Vector2 intersection;
		Vector2 point1, point2;
		bool isBounded = false;

		// 윗면 교차 체크
		point1 = rectanglePoints[0];
		point2 = rectanglePoints[2];
		if (VectorUtilities.GetIntersectionPoint(point1, point2, origin, line, out intersection))
		{
			Vector2 centerPoint = (point1 + point2) * 0.5f;

			intersectionPoint = intersection;

			if (IsInRectangle(intersection, origin, rectanglePoints))
			{
				return;
			}
			isBounded = centerPoint.Cross(intersection) < 0;
		}

		point1 = isBounded ? rectanglePoints[2] : rectanglePoints[0];
		point2 = isBounded ? rectanglePoints[3] : rectanglePoints[1];
		if (VectorUtilities.GetIntersectionPoint(point1, point2, origin, line, out intersection))
		{
			Vector2 centerPoint = (point1 + point2) * 0.5f;

			intersectionPoint = intersection;

			if (IsInRectangle(intersection, origin, rectanglePoints))
			{
				return;
			}
			isBounded = centerPoint.Cross(intersection) > 0;
		}

		point1 = rectanglePoints[1];
		point2 = rectanglePoints[2];
		if (VectorUtilities.GetIntersectionPoint(point1, point2, origin, line, out intersection))
		{
			Vector2 centerPoint = (point1 + point2) * 0.5f;

			intersectionPoint = intersection;

			if (IsInRectangle(intersection, origin, rectanglePoints))
			{
				return;
			}
			isBounded = centerPoint.Cross(intersection) > 0;
		}

		intersectionPoint = Vector2.zero;

		FDebug.LogWarning("[TruncatedBoxCollider] Do not Check to Point is Bounded");
	}

	protected override void OnDrawGizmos()
	{
		float originYPos = transform.position.y; 
		Vector2 originPos = new Vector2(transform.position.x, transform.position.z) + offset;
		Vector3 originPosVec3 = new Vector3(originPos.x, originYPos, originPos.y);
		Vector2[] rectanglePoints = GetRectanglePoints(originPos, transform.forward);
		Vector3[] rangeVectors = GetVectorToCut(originPosVec3);

		// 색상 지정
		Gizmos.color = Color.red;
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);


		// point 연산 및 Draw
		int pointCount = 0;
		List<Vector3> points = new List<Vector3> { new Vector3(rectanglePoints[pointCount].x, originYPos, rectanglePoints[pointCount].y) };

		while(pointCount++ < rectanglePoints.Length - 1)
		{
			points.Add(new Vector3(rectanglePoints[pointCount].x, originYPos, rectanglePoints[pointCount].y));

			Gizmos.DrawLine(points[pointCount - 1], points[pointCount]);
		}

		Gizmos.DrawLine(points[0], points[pointCount - 1]);

		Handles.DrawSolidRectangleWithOutline(points.ToArray(), colliderColor, Color.blue);

		// 공격 범위 표시
		/*Gizmos.DrawLine(originPosVec3, rangeVectors[0]);
		Gizmos.DrawLine(originPosVec3, rangeVectors[1]);*/

		// 공격 범위 자르기
		Vector2 intersectionPoint1, intersectionPoint2;
		GetIntersectionPoint(rectanglePoints, originPos, rangeVectors[0], out intersectionPoint1);
		GetIntersectionPoint(rectanglePoints, originPos, rangeVectors[1], out intersectionPoint2);

		Gizmos.DrawLine(originPosVec3, new Vector3(intersectionPoint1.x, originYPos, intersectionPoint1.y));
		Gizmos.DrawLine(originPosVec3, new Vector3(intersectionPoint2.x, originYPos, intersectionPoint2.y));
	}
#endif
}
