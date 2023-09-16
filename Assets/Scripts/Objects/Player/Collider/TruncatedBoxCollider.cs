using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TruncatedBoxCollider : TruncatedCollider<BoxCollider>
{
	public Vector2 offset;
	public Vector2 size;

	// �ݶ��̴� �⺻ ����
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

	// ���������� �� ���� ���� �������� �����Ͽ� ��ȯ��.
	private int GetintersectionPoint(Vector2[] rectanglePoints, Vector2 origin, Vector2 line, out Vector2[] intersectionPoint)
	{
		List<Vector2> points = new List<Vector2>();
		Vector2 intersection;
		bool isEnd = false;
		bool isRight = false;
		int dir = 0; // 0 : top, 1 : side, 2 : bottom

		// ���� ���� üũ
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

				intersection = isRight ? rectanglePoints[0] : rectanglePoints[2];
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

		/*	// ���鿡 �����ߴ��� üũ ���� ���ϸ� if�� ������
			if (!MathPlus.GetIntersectionPoint(targetPoint1, tagetPoint2, transform.position, line, out intersectionPoint))
		{
			pointCount = 3;
			// ���鿡 �����ߴ��� üũ ���� ���ϸ� if�� ������
			if (!MathPlus.GetIntersectionPoint(topPoint1, topPoint2, transform.position, line, out intersectionPoint))
			{
				// �׳� ����(������)����
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
		var vecs = GetVectorToCut();
		var rectanglePoints = GetRectanglePoints(origin, new Vector2(transform.forward.x, transform.forward.z));

		Vector3 leftLine = vecs[0] + origin;
		Vector3 rightLine = vecs[1] + origin;

		// ���� ��
		Handles.color = new Color(colliderColor.r, colliderColor.g, colliderColor.b, 0.1f);
		Gizmos.DrawLine(leftLine, origin);
		Gizmos.DrawLine(rightLine, origin);

		// ����
		Vector2[] intersectionPoint1, intersectionPoint2;
		int dir = GetintersectionPoint(rectanglePoints, new Vector2(origin.x, origin.z), new Vector2(leftLine.x, leftLine.z), out intersectionPoint1);
		GetintersectionPoint(rectanglePoints, new Vector2(origin.x, origin.z), new Vector2(rightLine.x, rightLine.z), out intersectionPoint2);

		if(dir >= 0)
		{
			Gizmos.DrawLine(new Vector3(intersectionPoint1[0].x, transform.position.y, intersectionPoint1[0].y), new Vector3(intersectionPoint2[0].x, transform.position.y, intersectionPoint2[0].y));
		}

		if (dir >= 1)
		{
			Gizmos.DrawLine(new Vector3(rectanglePoints[2].x, transform.position.y, rectanglePoints[2].y), new Vector3(intersectionPoint1[1].x, transform.position.y, intersectionPoint1[1].y));
			Gizmos.DrawLine(new Vector3(rectanglePoints[0].x, transform.position.y, rectanglePoints[0].y), new Vector3(intersectionPoint2[1].x, transform.position.y, intersectionPoint2[1].y));
		}

		/*if (dir >= 0)
		{
			Gizmos.DrawLine(new Vector3(intersectionPoint1.x, transform.position.y, intersectionPoint1.y), new Vector3(intersectionPoint2.x, transform.position.y, intersectionPoint2.y));
		}*/

		// �¿�

		// �ϴ�
	}
#endif
}
