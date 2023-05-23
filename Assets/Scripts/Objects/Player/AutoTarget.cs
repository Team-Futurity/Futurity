using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AutoTarget : Singleton<AutoTarget>
{
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
		origin.transform.LookAt(target.transform);
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
