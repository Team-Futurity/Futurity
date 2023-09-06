using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoTarget : Singleton<AutoTarget>
{
	private const int MaxAngle = 360; 
	private SortedDictionary<float, GameObject> ascendingDistances = new SortedDictionary<float, GameObject>();

	// origin과 오브젝트와의 거리를 Key로 해당 오브젝트의 인덱스를 Dictionary 형태로 저장
	private void SetDistance(GameObject[] objectList, GameObject origin)
	{
		ascendingDistances.Clear();

		for (int length = 0; length < objectList.Length; length++)
		{
			float distance = (objectList[length].transform.position - origin.transform.position).sqrMagnitude;

			ascendingDistances.Add(distance, objectList[length]);
		}
	}

	// 공격 범위 거르기
	public GameObject[] GetObjectsInAttackRange(GameObject[] objectList, RadiusCapsuleCollider collider)
	{
		if (objectList.Length == 0) { return null; }

		List<GameObject> list = new List<GameObject>();
		for (int length = 0; length < objectList.Length; length++)
		{
			if (collider.IsInCollider(objectList[length]))
			{
				list.Add(objectList[length]);
			}
		}

		return list.ToArray();
	}

	// 공격 사거리 거르기
	private GameObject[] GetObjectsInAttackLength(float attackLength)
	{
		if (ascendingDistances.Count == 0) { return null; }

		return ascendingDistances.Where(value => value.Key < attackLength).Select(value => value.Value).ToArray();
	}

	// 조준 범위 거르기
	private GameObject GetObjectInTargetRange(GameObject[] objectList, GameObject origin, float halfAngle)
	{
		if(objectList.Length == 0) { return null; }
		if(objectList.Length == 1) { return objectList[0]; }

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

	public void TurnToAutoTargetedObject(List<GameObject> objects, GameObject origin, RadiusCapsuleCollider attackCollider, float autoTargetAngle)
	{
		if(objects.Count == 0) { return; }
		if(autoTargetAngle > MaxAngle) { autoTargetAngle %= MaxAngle; }

		float halfAngle = autoTargetAngle * 0.5f;
		GameObject[] objectsArray = objects.ToArray();
		GameObject[] candidateObjects = GetObjectsInAttackRange(objectsArray, attackCollider);

		// 공격 범위 내에 있는 경우는 무시
		if(candidateObjects.Length > 0)
		{
			return;
			/*SetDistance(ObjectsInAttackRange, origin);
			int[] ascendingIndexes = ascendingDistances.Values.ToArray();

			TurnToTarget(objects[ascendingIndexes[0]], origin);*/
		}

		candidateObjects = GetObjectsInAttackLength(attackCollider.radius);
		if (candidateObjects.Length > 0)
		{
			TurnToTarget(candidateObjects[0], origin);
			return;
		}

		GameObject finalTarget = GetObjectInTargetRange(objectsArray, origin, halfAngle);

		if (finalTarget != null)
		{
			TurnToTarget(finalTarget, origin);
		}
		else
		{
			FDebug.LogWarning("[AutoTarget]Target Is NULL");
		}
	}
}
