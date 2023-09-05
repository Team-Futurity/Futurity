using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoTarget : Singleton<AutoTarget>
{
	// 공격 범위 거르기

	// 공격 사거리 거르기

	// 조준 범위 거르기
    public GameObject GetNearstObject(List<GameObject> objs, GameObject origin)
	{
		List<GameObject> list = objs.ToList();
		float distance = (list[0].transform.position - origin.transform.position).magnitude;
		GameObject nearstObj = list[0];
		list.RemoveAt(0);

		foreach(GameObject obj in list)
		{
			if(obj != null)
			{
				float curDistance = (obj.transform.position - origin.transform.position).magnitude;
				if (curDistance < distance)
				{
					distance = curDistance;
					nearstObj = obj;
				}
			}
		}

		return nearstObj;
	}

	public void TurnToTarget(GameObject target, GameObject origin)
	{
		Vector3 targetVec = target.transform.position;
		targetVec.y = origin.transform.position.y;

		origin.transform.LookAt(targetVec);
	}

	public void TurnToNearstObject(List<GameObject> objs, GameObject origin)
	{
		GameObject target = GetNearstObject(objs, origin);

		if (target != null)
		{
			TurnToTarget(target, origin);
		}
		else
		{
			FDebug.LogWarning("[AutoTarget]Target Is NULL");
		}
	}
}
