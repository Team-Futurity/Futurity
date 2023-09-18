using System;
using UnityEngine;

public static class MathPlus
{
	public const float cm2m = 1E-2F; // centimeter To meter
	public const int m2cm = 100; // meter To centimeter

	public static double Cross(this Vector2 first, Vector2 second)
	{
		return first.x * second.y - first.y * second.x;
	}
	public static void RotateToDirection(this Vector2 originPoint, Vector2 direction)
	{
		float rotationAngle = Vector2.SignedAngle(originPoint, direction) * Mathf.Deg2Rad;

		float cosAngle = Mathf.Cos(rotationAngle);
		float sinAngle = Mathf.Sin(rotationAngle);

		float x = originPoint.x;
		float y = originPoint.y;

		originPoint.x = x * cosAngle - y * sinAngle;
		originPoint.y = x * sinAngle + y * cosAngle;
	}

	public static bool GetIntersectionPoint(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, out Vector2 vec)
	{
		vec = Vector2.zero;
		Vector2 vec1 = (point2 - point1);
		Vector2 vec2 = (point4 - point3);
		double cross = vec1.Cross(vec2);
		
		if(Math.Abs((float)cross) < Mathf.Epsilon) { return false; }

		vec = point1 + vec1 * (float)((point3 - point1).Cross(vec2) / cross);

		return true;
	}
}
