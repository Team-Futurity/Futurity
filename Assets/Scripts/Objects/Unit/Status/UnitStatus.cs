using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatus", menuName = "Status/UnitStatus", order = 0)]
public class UnitStatus : ScriptableObject
{
	[SerializeField] private List<StatusData> status;
	[Space(10)] 
	[SerializeField] private List<StatusData> copyStatus;
	
	
	private void OnEnable()
	{
		CopyOrigin();
	}

	#region Private

	private void CopyOrigin()
	{
		if (copyStatus is not null)
		{
			copyStatus.Clear();

			foreach (var stat in status)
			{
				StatusData newStatus = new StatusData();

				newStatus.type = stat.type;
				newStatus.SetValue(stat.GetValue());

				copyStatus.Add(newStatus);
			}
		}
	}

	#endregion

	public void AutoGenerator()
	{
		var statusTypeList = Enum.GetValues(typeof(StatusType)).Cast<StatusType>();
		
		foreach (var statusType in statusTypeList)
		{
			if (!HasStatus(statusType))
			{
				if (statusType is StatusType.NONE or StatusType.MAX)
				{
					continue;
				}

				status.Add(new StatusData(statusType));
			}
		}
		CopyOrigin();
	}

	public void SetStatus(List<StatusData> statusDatas)
	{
		if (statusDatas is not null)
		{
			copyStatus = statusDatas.ToList();
		}
	}

	public void PlusStatus(List<StatusData> statusDatasList)
	{
		if (statusDatasList is not null)
		{
			foreach (var statusData in statusDatasList)
			{
				if (!HasStatus(statusData.type))
				{
					continue;
				}
				
				GetStatus(statusData.type).PlusValue(statusData.GetValue());
			}
		}
	}

	public void PlusStatus(StatusData statusData)
	{
		if (statusData is not null)
		{
			if (HasStatus(statusData.type))
			{
				GetStatus(statusData.type).PlusValue(statusData.GetValue());
			}
		}
	}

	public void MinusStatus(List<StatusData> statusDatasList)
	{
		if (statusDatasList is not null)
		{
			foreach (var statusData in statusDatasList)
			{
				if (!HasStatus(statusData.type))
				{
					continue;
				}
				
				GetStatus(statusData.type).MinusValue(statusData.GetValue());
			}
		}
	}

	public void MinusStatus(StatusData statusData)
	{
		if (statusData is not null)
		{
			if (HasStatus(statusData.type))
			{
				GetStatus(statusData.type).MinusValue(statusData.GetValue());
			}
		}
	}

	// 해당 Status를 가지고 있는지 확인
	public bool HasStatus(StatusType type)
	{
		foreach (var status in copyStatus)
		{
			if (status.type == type)
			{
				return true;
			}
		}
		return false;
	}

	// 해당 Status의 데이터를 가져온다.
	public StatusData GetStatus(StatusType type)
	{
		// 현재 Status에 해당 Status Type이 있는지 확인
		var hasStatus = HasStatus(type);

		if (!hasStatus)
		{
			FDebug.Log($"{type}에 해당하는 Status Key가 존재하지 않습니다.");
			return null;
		}

		return copyStatus.Find(x => x.type == type);
	}
}