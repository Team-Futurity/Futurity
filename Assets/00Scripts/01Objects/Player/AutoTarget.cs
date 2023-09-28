using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoTarget : Singleton<AutoTarget>
{
	private const int MaxAngle = 360; 

	private bool isMoving = false;		// 현재 움직임 명령이 내려졌는가
	private GameObject targetToMove;	// 목표
	private GameObject movingObject;	// 이동할 오브젝트
	private Vector3 targetPos;			// 목표의 위치
	private float margin;				// 멈춰설 거리(m)
	private float timeSpent;			// 소모 시간

	private void Start()
	{
		isMoving = false;
		StartCoroutine(MoveToTargetCoroutine());
	}

	// origin과 오브젝트와의 거리를 Key로 해당 오브젝트의 인덱스를 Dictionary 형태로 저장
	private ObjectDistanceInCollection[] GetObjectDistance(GameObject[] objectList, GameObject origin)
	{
		if(objectList.Length == 0) return null;

		ObjectDistanceInCollection[] distances = new ObjectDistanceInCollection[objectList.Length];

		for (int length = 0; length < distances.Length; length++)
		{
			float distance = (objectList[length].transform.position - origin.transform.position).sqrMagnitude;

			distances[length].SetUp(length, distance);
		}

		return distances;
	}

	// 공격 범위 거르기
	public GameObject[] GetObjectsInAttackRange(GameObject[] objectList, TruncatedCapsuleCollider collider)
	{
		if (objectList.Length == 0) { return null; }

		List<GameObject> list = new List<GameObject>();
		for (int length = 0; length < objectList.Length; length++)
		{
			if (collider.IsInCuttedCollider(objectList[length]))
			{
				list.Add(objectList[length]);
			}
		}

		return list.ToArray();
	}

	// 공격 사거리 거르기
	private int GetObjectsInAttackLength(ObjectDistanceInCollection[] distances, float attackLength)
	{
		if (distances.Length == 0) { FDebug.LogError("[AutoTarget] Distances Count is Zero."); return -1; }

		ObjectDistanceInCollection[] objectsInAttackLength = distances.Where(value => value.distance < attackLength).OrderBy(value => value.distance).ToArray();

		if(objectsInAttackLength.Length == 0) { return -1; }

		return objectsInAttackLength[0].index;
	}

	// 조준 범위 거르기
	private GameObject GetObjectInTargetRange(GameObject[] objectList, GameObject origin, float halfAngle)
	{
		if(objectList.Length == 0) { return null; }

		float cos = Mathf.Cos(halfAngle);

		List<GameObject> list = new List<GameObject>();
		float biggestDot = -10;
		GameObject closestObject = null;
		foreach (GameObject obj in objectList)
		{
			float dot = Vector3.Dot(origin.transform.forward, (obj.transform.position - origin.transform.position).normalized);

			if (dot >= cos)
			{
				list.Add(obj);

				if(dot > biggestDot)
				{
					biggestDot = dot;
					closestObject = obj;
				}
			}
		}

		return closestObject;
	}

	public void TurnToTarget(GameObject target, GameObject origin)
	{
		Vector3 targetVec = target.transform.position;
		targetVec.y = origin.transform.position.y;

		origin.transform.LookAt(targetVec);
	}

	/// <summary>
	/// Target에게 이동하는 코드
	/// </summary>
	/// <param name="target">대상 오브젝트</param>
	/// <param name="origin">이동할 오브젝트</param>
	/// <param name="margin">얼마만큼 앞에서 멈출지(cm)</param>
	/// <param name="time">소모할 시간</param>
	public void MoveToTarget(Vector3 targetPosition, GameObject origin, float margin, float time)
	{
		movingObject = origin;
		targetPos = targetPosition;
		this.margin = margin * MathPlus.cm2m;
		timeSpent = time;
		isMoving = true;
	}

	private IEnumerator MoveToTargetCoroutine()
	{
		while (true)
		{
			if (isMoving)
			{
				float timeRatio = Time.deltaTime / timeSpent;
				movingObject.transform.position = Vector3.Lerp(movingObject.transform.position, targetPos, timeRatio);

				float distance = Vector3.Distance(movingObject.transform.position, targetPos);
				if (distance <= margin)
				{
					Vector3 forward = -(movingObject.transform.position - targetPos).normalized;
					movingObject.transform.position = targetPos - forward * margin;
					isMoving = false;
				}
			}

			yield return null;
		}
	}

	/// <summary>
	/// 정해진 조건에 따라 회전, 이동을 수행하는 자동 조준 시스템
	/// </summary>
	/// <param name="objects">공격 콜라이더와 자동조준 콜라이더에서 검출된 오브젝트 일체</param>
	/// <param name="origin">행위자(회전, 이동을 수행할 대상)</param>
	/// <param name="attackCollider">공격 콜라이더</param>
	/// <param name="autoTargetAngle">자동 조준할 각도</param>
	/// <param name="margin">이동 시 멈춰설 거리</param>
	/// <param name="time">이동 시 이동에 소모하는 시간</param>
	/// <returns>이동을 수행했는가</returns>
	public bool AutoTargetProcess(List<GameObject> objects, GameObject origin, TruncatedCapsuleCollider attackCollider, float autoTargetAngle, float margin, float time, bool isMovable)
	{
		if(objects.Count == 0) { return false; }
		if(autoTargetAngle > MaxAngle) { autoTargetAngle %= MaxAngle; }

		float halfAngle = autoTargetAngle * 0.5f;
		GameObject[] objectsArray = objects.Distinct().ToArray();
		GameObject[] objectInAttackRange = GetObjectsInAttackRange(objectsArray, attackCollider);

		// 공격 범위 내에 있는 경우는 무시
		if(objectInAttackRange.Length > 0)
		{
			return false;
			/*SetDistance(ObjectsInAttackRange, origin);
			int[] ascendingIndexes = ascendingDistances.Values.ToArray();

			TurnToTarget(objects[ascendingIndexes[0]], origin);*/
		}

		// 상대적 거리 연산
		ObjectDistanceInCollection[] distances = GetObjectDistance(objectsArray, origin);

		// 공격 사거리에 있는 경우 그 방향으로 회전
		int nearestIndex = GetObjectsInAttackLength(distances, attackCollider.Radius);
		if(nearestIndex >= 0)
		{
			GameObject objectInAttackLength = objects[nearestIndex];
			if (objectInAttackLength != null)
			{
				TurnToTarget(objectInAttackLength, origin);
				return false;
			}
		}

		// 조준 범위 내에 있는지 체크
		GameObject objectInTargetRange = GetObjectInTargetRange(objectsArray, origin, halfAngle);

		// 조준범위 내에 있으면 회전 및 이동
		if (objectInTargetRange != null)
		{
			TurnToTarget(objectInTargetRange, origin);
			if(isMovable)
			{
				MoveToTarget(objectInTargetRange.transform.position, origin, margin, time);
			}
			
			return true;
		}
		else // 조준범위 내에 없는 경우는 코딩 잘못한 거  
		{
			FDebug.LogWarning("[AutoTarget]Target Is NULL");
		}

		return false;
	}
}
