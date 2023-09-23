using System;
using UnityEngine;

public static class VectorUtilities
{
	public static double Cross(this Vector2 first, Vector2 second)
	{
		return first.x * second.y - first.y * second.x;
	}

	public static Vector3 Vector2To3UsingZ(this Vector2 origin, float yPos = 0)
	{
		return new Vector3(origin.x, yPos, origin.y);
	}

	public static Vector2 Vector3To2RemoveY(this Vector3 origin)
	{
		return new Vector2(origin.x, origin.z);
	}

	/// <summary>
	/// 벡터 사이의 각을 반환합니다. PI가 넘어가는 값은 0~PI내의 값으로 치환됩니다.
	/// </summary>
	/// <param name="vector1">벡터1</param>
	/// <param name="vector2">벡터2</param>
	/// <returns>각도(0~180도)</returns>
	public static float GetAngleBetweenVectors(this Vector2 vector1, Vector2 vector2)
	{
		float vec1Magnitude = vector1.magnitude;
		float vec2Magnitude = vector2.magnitude;
		float inner = Vector2.Dot(vector1, vector2);
		float denominator = (vec1Magnitude * vec2Magnitude);

		if(denominator == 0) { return 0; }

		float angle = Mathf.Acos(inner / denominator);

		return angle * Mathf.Rad2Deg;
	}

	/// <summary>
	/// 벡터 사이의 각을 Standard를 기준으로 계산합니다. 2PI의 범위를 넘어가는 값은 0~2PI내의 값으로 치환됩니다.
	/// </summary>
	/// <param name="target">각도를 계산할 대상 벡터</param>
	/// <param name="standard">각도를 계산할 때 사용할 기준 벡터</param>
	/// <returns>각도(0~359도)</returns>
	public static float GetAngleWithStandard(this Vector2 target, Vector2? standard = null)
	{
		Vector2 standardVector = standard ?? Vector2.right;

		float angle = GetAngleBetweenVectors(target, standardVector);

		bool over180Degree = standardVector.Cross(target) < 0;

		return over180Degree ? MathPlus.MaxAngle - angle : angle;
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

		if (Math.Abs((float)cross) < Mathf.Epsilon) { return false; }

		vec = point1 + vec1 * (float)((point3 - point1).Cross(vec2) / cross);

		return true;
	}

	public static bool TryParseVector3(this string vector, out Vector3 result)
	{
		//
		//string[] vecDatas = vector.Replace("(", "").Replace(")", "").RemoveWhitespaces().Split(",");
		result = Vector3.zero;
/*
		if (vecDatas.Length != 3) { FDebug.LogWarning($"Inputed string is not Vector3 or Proper Value: {vector}", typeof(VectorUtilities)); return false; }

		float x, y, z;
		if (float.TryParse(vecDatas[0], out x) &&
			float.TryParse(vecDatas[1], out y) &&
			float.TryParse(vecDatas[2], out z))
		{
			result = new Vector3(x, y, z);
			return true;
		}*/

		return false;
	}
}