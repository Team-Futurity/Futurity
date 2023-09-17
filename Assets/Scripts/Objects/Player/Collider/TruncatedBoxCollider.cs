using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
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




#if UNITY_EDITOR
	// 0 : leftTop
	// 1 : leftBottom
	// 2 : rightTop
	// 3 : rightBottom
	private Vector2[] GetRectanglePoints(Vector2 origin, Vector2 forward)
	{
		Vector2[] points = new Vector2[4];

		float halfSizeX = size.x * 0.5f;
		float halfSizeZ = size.y * 0.5f;

		points[0] = new Vector2(-halfSizeX, halfSizeZ);
		points[1] = new Vector2(-halfSizeX, -halfSizeZ);
		points[2] = new Vector2(halfSizeX, halfSizeZ);
		points[3] = new Vector2(halfSizeX, -halfSizeZ);

		for (int i = 0; i < 4; i++)
		{
			points[i] += origin;
			points[i].RotateToDirection(forward);
		}

		return points;
	}

	// 점을 모두 저장하고 충돌한 면이 어딘지를 반환함
	// 0 : top, 1 : side, 2 : bottom
	private int GetintersectionPoint(Vector2[] rectanglePoints, Vector2 origin, Vector2 line, out Vector2[] intersectionPoint)
	{
		List<Vector2> points = new List<Vector2>();
		Vector2 intersection;
		bool isEnd = false;
		bool isRight = false;
		int dir = 0;

		// 윗면 교차 체크
		if (MathPlus.GetIntersectionPoint(rectanglePoints[0], rectanglePoints[2], origin, line, out intersection))
		{
			Vector2 centerPoint = (rectanglePoints[0] + rectanglePoints[2]) * 0.5f;

			Vector2 originToIS = origin - intersection;
			Vector2 originToLeftTop = origin - rectanglePoints[0];
			Vector2 originToRightTop = origin - rectanglePoints[2];

			isEnd = !(originToIS.magnitude > originToRightTop.magnitude || originToIS.magnitude > originToLeftTop.magnitude);
			dir = 0;

			if (!isEnd)
			{
				isRight = centerPoint.Cross(intersection) < 0;

				intersection = isRight ? rectanglePoints[2] : rectanglePoints[0];
			}

			points.Add(new Vector2(intersection.x, intersection.y));
		}

		Vector2[] sidePoints = new Vector2[2];
		sidePoints[0] = isRight ? rectanglePoints[2] : rectanglePoints[0];
		sidePoints[1] = isRight ? rectanglePoints[3] : rectanglePoints[1];

		if (!isEnd && MathPlus.GetIntersectionPoint(sidePoints[0], sidePoints[1], origin, line, out intersection))
		{
			dir = 1;

			points.Add(new Vector2(intersection.x, intersection.y));
		}

		intersectionPoint = points.ToArray();

		return dir;

		/*	// 옆면에 교차했는지 체크 교차 못하면 if문 안으로
			if (!MathPlus.GetIntersectionPoint(targetPoint1, tagetPoint2, transform.position, line, out intersectionPoint))
		{
			pointCount = 3;
			// 윗면에 교차했는지 체크 교차 못하면 if문 안으로
			if (!MathPlus.GetIntersectionPoint(topPoint1, topPoint2, transform.position, line, out intersectionPoint))
			{
				// 그냥 원점(시작점)으로
				intersectionPoint = transform.position;
				pointCount = 4;
			}
		}*/

		//return pointCount;
	}

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
		/*Gizmos.color = colliderColor;
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
		*//*Handles.DrawPolyLine(Vector2ToVector3(finalPoints));*/

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
		int dir = GetintersectionPoint(rectanglePoints, originVec2, originVec2, out intersectionPoint1);
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


		/*if (dir >= 0)
		{
			Gizmos.DrawLine(new Vector3(intersectionPoint1.x, transform.position.y, intersectionPoint1.y), new Vector3(intersectionPoint2.x, transform.position.y, intersectionPoint2.y));
		}*/

		// 좌우

		// 하단
	}
#endif
}
