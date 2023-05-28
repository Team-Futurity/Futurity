using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "OriginStatus", menuName = "Status/OriginStatus", order = 0)]
public class OriginStatus : ScriptableObject
{
	[SerializeField] private List<StatusData> status = new List<StatusData>();
	
	public void AutoGenerator()
	{
		if (status is not null)
		{
			status.Clear();
		}

		var statusTypeList = Enum.GetValues(typeof(StatusType)).Cast<StatusType>();
		
		foreach (var statusType in statusTypeList)
		{
			if (statusType is StatusType.NONE or StatusType.MAX)
			{
				continue;
			}

			status.Add(new StatusData(statusType));
		}
	}

	public List<StatusData> GetStatus()
	{
		return status;
	}

	public StatusData GetStatusElement(StatusType type)
	{
		foreach(var e in status)
		{
			if(e.type == type)
			{
				return e;
			}
		}

		return null;
	}
}